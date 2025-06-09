using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using UOP.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace UOP.Infrastructure.Repositories
{
    public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TPrimaryKey : struct
    {
        internal readonly DbSet<TEntity> _dbSet;
        internal readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _dbSet = context.Set<TEntity>();
            _context = context;
        }
        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<TEntity> GetByIdAsync(TPrimaryKey id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _dbSet.Update(entity);
            }, cancellationToken);
        }

        public async Task RemoveAsync(TPrimaryKey id, CancellationToken cancellationToken)
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            _dbSet.Remove(entity);
        }

        public IQueryable<TEntity> GetByIdQueryable(TPrimaryKey id)
        {
            var keyName = _context.Model.FindEntityType(typeof(TEntity))
                .FindPrimaryKey().Properties
                .Single().Name;

            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var property = Expression.Property(parameter, keyName);
            var value = Expression.Constant(id);
            var equals = Expression.Equal(property, value);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equals, parameter);

            return _dbSet.Where(lambda);
        }

        public IQueryable<TEntity> GetByIdQueryable(TEntity entity)
        {
            var keyName = _context.Model.FindEntityType(typeof(TEntity))
                .FindPrimaryKey().Properties
                .Single().Name;

            var id = (TPrimaryKey)typeof(TEntity).GetProperty(keyName).GetValue(entity);
            return GetByIdQueryable(id);
        }

    }
}
