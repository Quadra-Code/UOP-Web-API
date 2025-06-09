using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UOP.Domain.Interfaces
{
    public interface IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TPrimaryKey : struct
    {
        IQueryable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<TEntity> GetByIdAsync(TPrimaryKey id, CancellationToken cancellationToken = default);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task RemoveAsync(TPrimaryKey id, CancellationToken cancellationToken = default);
        IQueryable<TEntity> GetByIdQueryable(TPrimaryKey id);
        IQueryable<TEntity> GetByIdQueryable(TEntity entity);
    }
}
