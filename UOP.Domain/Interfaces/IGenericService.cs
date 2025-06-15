using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UOP.Domain.Models;

namespace UOP.Domain.Interfaces
{
    public interface IGenericService<TEntity, TCreateEntityDTO, TEntityDTO, TPrimaryKey>
        where TEntity : class, new()
        where TCreateEntityDTO : class, new()
        where TEntityDTO : class, new()
        where TPrimaryKey : struct
    {
        Result<IQueryable<TEntityDTO>> GetAll(int pageNumber = 1, int pageSize = int.MaxValue, string orderBy = "Id", string orderDirection = "asc");
        Result<IQueryable<TEntityDTO>> GetByFilter(
            Expression<Func<TEntity, bool>>? filter,
            int pageNumber = 1,
            int pageSize = int.MaxValue,
            string orderBy = "Id",
            string orderDirection = "asc"
            );

        Task<Result<PaginatedResponse<TEntityDTO>>> GetAllAsync(
            int pageNumber = 1,
            int pageSize = int.MaxValue,
            string orderBy = "Id",
            string orderDirection = "asc",
            CancellationToken cancellationToken = default);

        Task<Result<PaginatedResponse<TEntityDTO>>> GetByFilterAsync(
            Expression<Func<TEntity, bool>>? filter,
            int pageNumber = 1,
            int pageSize = int.MaxValue,
            string orderBy = "Id",
            string orderDirection = "asc",
            CancellationToken cancellationToken = default);

        Task<Result<TEntityDTO>>? GetByIdAsync(TPrimaryKey id, CancellationToken cancellationToken = default);
        Task<Result<TEntityDTO>> AddAsync(TCreateEntityDTO entityDTO, CancellationToken cancellationToken = default);
        Task<Result<TEntityDTO>> UpdateAsync(TEntityDTO entityDTO, CancellationToken cancellationToken = default);
        Task<Result<bool>> RemoveAsync(TPrimaryKey id, CancellationToken cancellationToken = default);
    }
}
