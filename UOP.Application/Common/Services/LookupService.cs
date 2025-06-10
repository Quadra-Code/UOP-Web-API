using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UOP.Application.Common.DTOs;
using UOP.Application.Common.Interfaces;
using UOP.Domain.Interfaces;
using UOP.Domain.Models;

namespace UOP.Application.Common.Services
{
    public class LookupService<TEntity>(IRepository<TEntity, Guid> repository) : ILookupService<TEntity> where TEntity : class, IEntity<Guid>
    {
        private readonly IRepository<TEntity, Guid> _repository = repository;

        public async Task<PaginatedResponse<LookupDTO<Guid>>> GetLookupAsync(
            string? search = null,
            int pageIndex = 1,
            int pageSize = 10,
            string orderBy = "Order",
            string orderDirection = "asc",
            Expression<Func<TEntity, bool>> filters = null,
            CancellationToken cancellationToken = default)
        {
            var query = _repository.GetAll();

            // Apply filters if any
            if (filters != null)
            {
                query = query.Where(filters);
            }

            var mappedQuery = query.ProjectToType<LookupDTO<Guid>>();
            // Apply search if provided
            if (!string.IsNullOrEmpty(search))
            {
                mappedQuery = mappedQuery.Where(a => a.NameEn.Contains(search) || a.NameAr.Contains(search));
            }

            // Define a dictionary for mapping property names to expressions with case-insensitive lookup
            var orderByMappings = new Dictionary<string, Expression<Func<LookupDTO<Guid>, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            { "NameEn", x => x.NameEn },
            { "NameAr", x => x.NameAr },
            { "Order", x => x.Order },
            { "Id", x => x.Id }
        };

            // Apply ordering if provided
            if (!string.IsNullOrEmpty(orderBy) && orderByMappings.TryGetValue(orderBy, out var orderByExpression))
            {
                mappedQuery = string.Equals(orderDirection, "desc", StringComparison.OrdinalIgnoreCase)
                    ? mappedQuery.OrderByDescending(orderByExpression)
                    : mappedQuery.OrderBy(orderByExpression);
            }

            var totalCount = await mappedQuery.CountAsync(cancellationToken);

            var items = await mappedQuery
            .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResponse<LookupDTO<Guid>>(items, totalCount, pageIndex, pageSize);
        }
    }
}
