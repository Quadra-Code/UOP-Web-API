using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using UOP.Domain.Extensions;
using UOP.Domain.Interfaces;
using UOP.Domain.Models;

namespace UOP.Application.Common.Services
{
    public class GenericService<TEntity, TCreateEntityDTO, TEntityDTO, TPrimaryKey>(IRepository<TEntity, TPrimaryKey> repository, IUnitOfWork unitOfWork)
        : IGenericService<TEntity, TCreateEntityDTO, TEntityDTO, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>, new()
    where TEntityDTO : class, new()
    where TCreateEntityDTO : class, new()
    where TPrimaryKey : struct
    {
        protected readonly IRepository<TEntity, TPrimaryKey> Repository = repository;
        protected readonly IUnitOfWork UnitOfWork = unitOfWork;

        public virtual Result<IQueryable<TEntityDTO>> GetAll(int pageNumber = 1, int pageSize = int.MaxValue, string orderBy = "Id", string orderDirection = "asc")
        {
            try
            {
                var entities = ApplyPaginationAndSorting(Repository.GetAll(), pageNumber, pageSize, orderBy, orderDirection);
                return Result<IQueryable<TEntityDTO>>.Success(entities);
            }
            catch (Exception ex)
            {
                return Result<IQueryable<TEntityDTO>>.Failure(ex.Message);
            }
        }

        public virtual async Task<Result<PaginatedResponse<TEntityDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = int.MaxValue, string orderBy = "Id", string orderDirection = "asc", CancellationToken cancellationToken = default)
        {
            try
            {
                var query = Repository.GetAll();
                var entities = ApplyPaginationAndSorting(query, pageNumber, pageSize, orderBy, orderDirection);
                var totalCount = await query.CountAsync(cancellationToken);
                return Result<PaginatedResponse<TEntityDTO>>.Success(new PaginatedResponse<TEntityDTO>(entities.ToList(), totalCount, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result<PaginatedResponse<TEntityDTO>>.Failure(ex.Message);
            }
        }

        public virtual Result<IQueryable<TEntityDTO>> GetByFilter(Expression<Func<TEntity, bool>> filter, int pageNumber = 1, int pageSize = int.MaxValue, string orderBy = "Id", string orderDirection = "asc")
        {
            try
            {
                var query = Repository.GetAll();
                query = filter == null ? query : query.Where(filter);
                var entities = ApplyPaginationAndSorting(query, pageNumber, pageSize, orderBy, orderDirection);
                return Result<IQueryable<TEntityDTO>>.Success(entities);
            }
            catch (Exception ex)
            {
                return Result<IQueryable<TEntityDTO>>.Failure(ex.Message);
            }
        }

        public virtual async Task<Result<PaginatedResponse<TEntityDTO>>> GetByFilterAsync(Expression<Func<TEntity, bool>>? filter, int pageNumber = 1, int pageSize = int.MaxValue, string orderBy = "Id", string orderDirection = "asc", CancellationToken cancellationToken = default)
        {
            try
            {
                var query = Repository.GetAll();
                query = filter == null ? query : query.Where(filter);

                var entities = ApplyPaginationAndSorting(query, pageNumber, pageSize, orderBy, orderDirection);
                var totalCount = await query.CountAsync(cancellationToken);

                return Result<PaginatedResponse<TEntityDTO>>.Success(new PaginatedResponse<TEntityDTO>(entities.ToList(), totalCount, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result<PaginatedResponse<TEntityDTO>>.Failure(ex.Message);
            }
        }

        public virtual async Task<Result<TEntityDTO>> GetByIdAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = Repository.GetByIdQueryable(id);
                var entity = await query.ProjectToType<TEntityDTO>().FirstOrDefaultAsync(cancellationToken);

                return entity is not null
                    ? Result<TEntityDTO>.Success(entity)
                    : Result<TEntityDTO>.Failure($"Entity with id {id} not found.");
            }
            catch (Exception ex)
            {
                return Result<TEntityDTO>.Failure(ex.Message);
            }
        }

        public virtual async Task<Result<TEntityDTO>> AddAsync(TCreateEntityDTO entityDTO, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = entityDTO.Adapt<TEntity>();

                if (typeof(TPrimaryKey) == typeof(Guid))
                {
                    var idProperty = entity.GetType().GetProperty("Id");

                    if (idProperty is not null)
                    {
                        var currentIdValue = idProperty.GetValue(entity);


                        if (currentIdValue is Guid currentGuid && currentGuid == Guid.Empty)
                        {
                            idProperty.SetValue(entity, UuidV7.NewGuid());
                        }
                    }
                }

                if (entity is IAuditEntity<TPrimaryKey> audit)
                {
                    audit.CreatedBy = "System";
                    audit.CreatedDate = DateTime.Now;
                    audit.UpdatedBy = "System";
                    audit.UpdatedDate = DateTime.Now;
                }

                Debug.Assert(entity is not null, nameof(entity) + " is not null");
                await Repository.AddAsync(entity, cancellationToken);
                await UnitOfWork.SaveChangesAsync(cancellationToken);

                var query = Repository.GetByIdQueryable(entity);
                var createdEntity = await query.ProjectToType<TEntityDTO>().FirstOrDefaultAsync(cancellationToken);

                return Result<TEntityDTO>.Success(createdEntity);
            }
            catch (Exception ex)
            {
                return Result<TEntityDTO>.Failure(ex.Message);
            }
        }

        public virtual async Task<Result<TEntityDTO>> UpdateAsync(TEntityDTO entityDTO, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingEntity = await Repository.GetByIdAsync((TPrimaryKey)(entityDTO.GetType().GetProperty("Id")?.GetValue(entityDTO) ?? throw new InvalidOperationException()), cancellationToken);

                if (existingEntity == null)
                {
                    return Result<TEntityDTO>.Failure($"Entity with id {entityDTO.GetType().GetProperty("Id")?.GetValue(entityDTO)} not found.");
                }

                entityDTO.Adapt(existingEntity);

                await Repository.UpdateAsync(existingEntity, cancellationToken);
                await UnitOfWork.SaveChangesAsync(cancellationToken);
                var updatedEntity = await Repository.GetByIdQueryable(existingEntity.Id)
                   .ProjectToType<TEntityDTO>()
                   .FirstOrDefaultAsync(cancellationToken);
                return Result<TEntityDTO>.Success(updatedEntity);
            }
            catch (Exception ex)
            {
                return Result<TEntityDTO>.Failure(ex.Message);
            }
        }

        public virtual async Task<Result<bool>> RemoveAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingEntity = await Repository.GetByIdAsync(id, cancellationToken);

                if (existingEntity == null)
                {
                    return Result<bool>.Failure($"Entity with id {id} not found.");
                }
                await Repository.RemoveAsync(id, cancellationToken);
                await UnitOfWork.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }

        private IQueryable<TEntityDTO> ApplyPaginationAndSorting(IQueryable<TEntity> entities, int pageNumber, int pageSize, string orderBy, string orderDirection)
        {
            var propertyInfo = typeof(TEntity).GetProperty(orderBy, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (propertyInfo is not null)
            {
                var parameterExpr = Expression.Parameter(typeof(TEntity), "e");
                var propertyExpr = Expression.Property(parameterExpr, propertyInfo);
                var lambdaExpr = Expression.Lambda(propertyExpr, parameterExpr);
                var methodName = string.Equals(orderDirection, "desc", StringComparison.OrdinalIgnoreCase) ? "OrderByDescending" : "OrderBy";
                entities = entities.Provider.CreateQuery<TEntity>(
                    Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(TEntity), propertyInfo.PropertyType }, entities.Expression, lambdaExpr));
            }
            if (pageNumber > 0)
            {
                entities = entities.Skip((pageNumber - 1) * pageSize);
            }

            if (pageSize > 0)
            {
                entities = entities.Take(pageSize);
            }

            return entities.ProjectToType<TEntityDTO>();
        }
    }
}
