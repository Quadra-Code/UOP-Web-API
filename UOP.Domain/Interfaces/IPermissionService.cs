using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using UOP.Domain.Entities;
using UOP.Domain.Models;

namespace UOP.Domain.Interfaces
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(Guid userId, string permissionName, CancellationToken cancellationToken = default);

        Task GrantPermissionAsync(Guid userId, string permissionName, CancellationToken cancellationToken = default);

        Task RevokePermissionAsync(Guid userId, string permissionName, CancellationToken cancellationToken = default);

        Task<bool> IsPermissionGrantedAsync(Guid userId, string permissionName, CancellationToken cancellationToken = default);

        Task<Result<PaginatedResponse<PermissionDTO>>> GetUserPermissionsAsync(
            Guid userId,
            Guid tenantId,
            int pageIndex = 1,
            int pageSize = 10,
            string orderBy = "Name",
            string orderDirection = "asc");

        // New methods for role permissions
        Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId);
        Task<Permission?> GetPermissionByIdAsync(Guid permissionId);
        Task GrantPermissionToRoleAsync(Guid roleId, string permissionName);
        Task RevokePermissionFromRoleAsync(Guid roleId, string permissionName);
    }
}
