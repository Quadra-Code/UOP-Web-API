using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UOP.Domain.Models;

namespace UOP.Domain.Interfaces
{
    public interface IGenericService<TEntity, TCreateEntityDto, TEntityDto, TPrimaryKey>
        where TEntity : class, new()
        where TCreateEntityDto : class, new()
        where TEntityDto : class, new()
        where TPrimaryKey : struct
    {
        Result<IQueryable<TEntityDto>> GetAll(int pageNumber = 1, int pageSize = int.MaxValue, string orderBy = "Id", string orderDirection = "asc");
        Result<IQueryable<TEntityDto>> GetByFilter(
            Expression<Func<TEntity, bool>>? filter,
            int pageNumber = 1,
            int pageSize = int.MaxValue,
            string orderBy = "Id",
            string orderDirection = "asc"
            );

        Task<Result<PaginatedResponse<TEntityDto>>> GetAllAsync(
            int pageNumber = 1,
            int pageSize = int.MaxValue,
            string orderBy = "Id",
            string orderDirection = "asc",
            CancellationToken cancellationToken = default);

        Task<Result<PaginatedResponse<TEntityDto>>> GetByFilterAsync(
            Expression<Func<TEntity, bool>>? filter,
            int pageNumber = 1,
            int pageSize = int.MaxValue,
            string orderBy = "Id",
            string orderDirection = "asc",
            CancellationToken cancellationToken = default);

        Task<Result<TEntityDto>>? GetByIdAsync(TPrimaryKey id, CancellationToken cancellationToken = default);
        Task<Result<TEntityDto>> AddAsync(TCreateEntityDto entityDto, CancellationToken cancellationToken = default);
        Task<Result<TEntityDto>> UpdateAsync(TEntityDto entityDto, CancellationToken cancellationToken = default);
        Task<Result<bool>> RemoveAsync(TPrimaryKey id, CancellationToken cancellationToken = default);
    }
}
