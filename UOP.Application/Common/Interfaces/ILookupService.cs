using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UOP.Application.Common.DTOs;
using UOP.Domain.Interfaces;
using UOP.Domain.Models;

namespace UOP.Application.Common.Interfaces
{
    public interface ILookupService<TEntity> where TEntity : class, IEntity<Guid>
    {
        Task<PaginatedResponse<LookupDTO<Guid>>> GetLookupAsync(
            string? search = null,
            int pageIndex = 1,
            int pageSize = 10,
            string orderBy = "Order",
            string orderDirection = "asc",
            Expression<Func<TEntity, bool>>? filters = null,
            CancellationToken cancellationToken = default);
    }
}
