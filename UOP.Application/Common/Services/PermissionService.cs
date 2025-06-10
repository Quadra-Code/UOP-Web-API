using UOP.Domain.Entities;
using UOP.Domain.Extensions;
using UOP.Domain.Interfaces;
using UOP.Domain.Models;
using Mapster;

namespace UOP.Application.Common.Services
{
    //public class PermissionService : IPermissionService
    //{
    //    private readonly IRepository<Permission, Guid> _permissionRepository;
    //    private readonly IRepository<UserInRole, Guid> _userInRoleRepository;
    //    private readonly IRepository<RoleItemPermission, Guid> _roleItemPermissionRepository;
    //    private readonly IRepository<Item, Guid> _itemRepository;
    //    private readonly IRepository<ItemType, Guid> _itemTypeRepository;
    //    private readonly IUserRepository _userRepository;

    //    private readonly IUnitOfWork _unitOfWork;
    //    //private readonly IMapper _mapper;

    //    public PermissionService(
    //        IRepository<Permission, Guid> repository,
    //        IUnitOfWork unitOfWork,
    //        //IMapper mapper,
    //        IRepository<UserInRole, Guid> userInRoleRepository,
    //        IRepository<RoleItemPermission, Guid> roleItemPermissionRepository,
    //        IRepository<Item, Guid> itemRepository,
    //        IRepository<ItemType, Guid> itemTypeRepository,
    //        IUserRepository userRepository


    //        )
    //    {
    //        _permissionRepository = repository;
    //        _unitOfWork = unitOfWork;
    //        //_mapper = mapper;
    //        _userInRoleRepository = userInRoleRepository;
    //        _roleItemPermissionRepository = roleItemPermissionRepository;
    //        _itemRepository = itemRepository;
    //        _itemTypeRepository = itemTypeRepository;
    //        _userRepository = userRepository;

    //    }

    //    public async Task<bool> HasPermissionAsync(Guid userId, string permissionName, CancellationToken cancellationToken = default)
    //    {
    //        // Find roles for this user
    //        var userRoles = _userInRoleRepository.GetAll()
    //                              .Where(ur => ur.UserId == userId)
    //                              .Select(ur => ur.RoleId)
    //                              .ToList();

    //        var permission = _permissionRepository.GetAll().FirstOrDefault(p => p.Name.ToUpper() == permissionName.ToUpper());
    //        if (permission == null)
    //        {
    //            throw new Exception(string.Format("Invalid permission defined ${PermissionName}", permissionName));
    //        }

    //        // Check if any of these roles have the requested permission
    //        var isValid = _roleItemPermissionRepository.GetAll()
    //                      .Any(rp => (userRoles.Contains(rp.RoleId)) && rp.PermissionId == permission.PermissionId);

    //        return isValid;
    //    }

    //    public async Task GrantPermissionAsync(Guid userId, string permissionName, CancellationToken cancellationToken = default)
    //    {
    //        // Find the permission
    //        var permission = _permissionRepository.GetAll()
    //                                     .FirstOrDefault(p => p.Name.Equals(permissionName, StringComparison.OrdinalIgnoreCase));

    //        if (permission is not null)
    //        {
    //            // Optionally, you need to determine which role(s) to grant this permission
    //            var userRoles = _userInRoleRepository.GetAll()
    //                              .Where(ur => ur.UserId == userId)
    //                              .ToList();

    //            foreach (var userRole in userRoles)
    //            {
    //                var roleItemPermission = new RoleItemPermission
    //                {
    //                    RoleId = userRole.RoleId,
    //                    PermissionId = permission.PermissionId,
    //                    // Set other necessary properties for RoleItemPermission here
    //                    CreatedBy = userId.ToString(), // Or current user's ID
    //                    CreatedDate = DateTime.UtcNow,
    //                    ItemId = Guid.Empty, // Depending on your logic
    //                    ItemTypeId = Guid.Empty // Depending on your logic
    //                };
    //                await _roleItemPermissionRepository.AddAsync(roleItemPermission);
    //            }

    //            await _unitOfWork.SaveChangesAsync();
    //        }
    //        else
    //        {
    //            throw new Exception("Permission not found.");
    //        }
    //    }

    //    public async Task RevokePermissionAsync(Guid userId, string permissionName, CancellationToken cancellationToken = default)
    //    {
    //        // Find the permission
    //        var permission = _permissionRepository.GetAll()
    //                                     .FirstOrDefault(p => p.Name.Equals(permissionName, StringComparison.OrdinalIgnoreCase));

    //        if (permission is not null)
    //        {
    //            var userRoles = _userInRoleRepository.GetAll()
    //                              .Where(ur => ur.UserId == userId)
    //                              .Select(ur => ur.RoleId)
    //                              .ToList();

    //            foreach (var roleId in userRoles)
    //            {
    //                var roleItemPermission = _roleItemPermissionRepository
    //                              .GetAll()
    //                              .FirstOrDefault(rp => rp.RoleId == roleId &&
    //                                                    rp.PermissionId == permission.PermissionId);
    //                if (roleItemPermission is not null)
    //                {
    //                    await _roleItemPermissionRepository.RemoveAsync(roleItemPermission.RoleItemPermissionId);
    //                }
    //            }

    //            await _unitOfWork.SaveChangesAsync();
    //        }
    //        else
    //        {
    //            throw new Exception("Permission not found.");
    //        }
    //    }

    //    public async Task<Result<PaginatedResponse<PermissionDTO>>> GetUserPermissionsAsync(
    //        Guid userId,
    //        Guid tenantId,
    //        int pageIndex = 1,
    //        int pageSize = 10,
    //        string orderBy = "Name",
    //        string orderDirection = "asc")
    //    {

    //        var userRoles = _userInRoleRepository.GetAll()
    //                          .Where(ur => ur.UserId == userId && ur.OrganizationId == tenantId)
    //                          .Select(ur => ur.RoleId)
    //                          .ToList();

    //        var query = _roleItemPermissionRepository.GetAll()
    //                .Include(rp => rp.Permission.PermissionType)
    //                .Where(rp => userRoles.Contains(rp.RoleId))
    //                .Select(rp => rp.Permission);

    //        // Apply ordering
    //        query = orderBy?.ToLower() switch
    //        {
    //            "name" => orderDirection?.ToLower() == "desc"
    //                ? query.OrderByDescending(p => p.Name)
    //                : query.OrderBy(p => p.Name),
    //            "order" => orderDirection?.ToLower() == "desc"
    //                ? query.OrderByDescending(p => p.Order)
    //                : query.OrderBy(p => p.Order),
    //            _ => query.OrderBy(p => p.Name)
    //        };

    //        // Get total count
    //        var totalCount = await query.CountAsync();

    //        // Apply pagination
    //        var items = await query
    //            .Skip((pageIndex - 1) * pageSize)
    //            .Take(pageSize)
    //            .ToListAsync();

    //        var userPermissionsDTOs = _mapper.Map<List<PermissionDTO>>(items);

    //        var paginatedResponse = new PaginatedResponse<PermissionDTO>(
    //            userPermissionsDTOs,
    //            totalCount,
    //            pageIndex,
    //            pageSize
    //        );

    //        return Result<PaginatedResponse<PermissionDTO>>.Success(paginatedResponse);
    //    }

    //    public async Task<bool> IsPermissionGrantedAsync(Guid userId, string permissionName, CancellationToken cancellationToken = default)
    //    {
    //        return await HasPermissionAsync(userId, permissionName, cancellationToken);
    //    }

    //    public async Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId)
    //    {
    //        var permissions = _roleItemPermissionRepository.GetAll()
    //            .Where(rp => rp.RoleId == roleId)
    //            .Select(rp => rp.Permission)
    //            .ToList();

    //        return permissions;
    //    }

    //    public async Task<Permission?> GetPermissionByIdAsync(Guid permissionId)
    //    {
    //        return _permissionRepository.GetAll()
    //            .FirstOrDefault(p => p.PermissionId == permissionId);
    //    }

    //    public async Task GrantPermissionToRoleAsync(Guid roleId, string permissionName)
    //    {
    //        var permission = _permissionRepository.GetAll()
    //            .Where(p => p.Name.ToUpper() == permissionName.ToUpper())
    //            .FirstOrDefault();

    //        if (permission == null)
    //        {
    //            throw new Exception($"Permission '{permissionName}' not found.");
    //        }

    //        // Check if permission is already granted
    //        var existingPermission = _roleItemPermissionRepository.GetAll()
    //            .FirstOrDefault(rp => rp.RoleId == roleId && rp.PermissionId == permission.PermissionId);

    //        if (existingPermission is not null)
    //        {
    //            return; // Permission already granted
    //        }

    //        // Get first Item and ItemType using generic repository
    //        var items = await _itemRepository.GetAllAsync(CancellationToken.None);
    //        var itemTypes = await _itemTypeRepository.GetAllAsync(CancellationToken.None);

    //        var firstItem = items.FirstOrDefault();
    //        var firstItemType = itemTypes.FirstOrDefault();

    //        if (firstItem == null || firstItemType == null)
    //        {
    //            throw new Exception("Required Item or ItemType not found in the system.");
    //        }

    //        // Check if a RoleItemPermission with these exact values already exists
    //        var existingRoleItemPermission = _roleItemPermissionRepository.GetAll()
    //            .FirstOrDefault(rp =>
    //                rp.RoleId == roleId &&
    //                rp.PermissionId == permission.PermissionId &&
    //                rp.ItemId == firstItem.ItemId &&
    //                rp.ItemTypeId == firstItemType.ItemTypeId);

    //        if (existingRoleItemPermission is not null)
    //        {
    //            return; // This exact permission combination already exists
    //        }

    //        var roleItemPermission = new RoleItemPermission
    //        {
    //            RoleItemPermissionId = UuidV7.NewGuid(), // Explicitly set a new ID
    //            RoleId = roleId,
    //            PermissionId = permission.PermissionId,
    //            CreatedBy = "System", // You might want to pass the current user
    //            CreatedDate = DateTime.UtcNow,
    //            ItemId = firstItem.ItemId,
    //            ItemTypeId = firstItemType.ItemTypeId
    //        };

    //        await _roleItemPermissionRepository.AddAsync(roleItemPermission, CancellationToken.None);
    //        await _unitOfWork.SaveChangesAsync();
    //    }

    //    public async Task RevokePermissionFromRoleAsync(Guid roleId, string permissionName)
    //    {
    //        var permission = _permissionRepository.GetAll()
    //            .Where(p => p.Name.ToUpper() == permissionName.ToUpper())
    //            .FirstOrDefault();

    //        if (permission == null)
    //        {
    //            throw new Exception($"Permission '{permissionName}' not found.");
    //        }

    //        var roleItemPermission = _roleItemPermissionRepository.GetAll()
    //            .FirstOrDefault(rp => rp.RoleId == roleId && rp.PermissionId == permission.PermissionId);

    //        if (roleItemPermission is not null)
    //        {
    //            await _roleItemPermissionRepository.RemoveAsync(roleItemPermission.RoleItemPermissionId);
    //            await _unitOfWork.SaveChangesAsync();
    //        }
    //    }
    //}

}
