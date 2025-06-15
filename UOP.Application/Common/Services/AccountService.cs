using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UOP.Application.Common.DTOs.User;
using UOP.Application.Interfaces;
using UOP.Domain.Entities;
using UOP.Domain.Interfaces;
using UOP.Domain.Models;

namespace UOP.Application.Common.Services
{
    public class AccountService : GenericService<User, AccountCreateDTO, UserDTO, Guid>, IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<Role> roleManager;
        public AccountService(IRepository<User, Guid> repository, IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager) : base(repository, unitOfWork)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public Task<Result<UserDTO>> CreateAdminAsync(CreateAdminDTO createAdminDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<UserDTO>> LoginAsync(AccountLoginDTO accountLoginDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(accountLoginDTO.Email);
                if (user is not null)
                {
                    var result = await userManager.CheckPasswordAsync(user, accountLoginDTO.Password);
                    if (result)
                    {
                        return Result<UserDTO>.Success(user.Adapt<UserDTO>());
                    }
                }
                return Result<UserDTO>.Failure("Login Failed. Invalid Credentials");
            }
            catch (Exception ex)
            {
                return Result<UserDTO>.Failure(ex.Message);
            }
        }

        // Register new client account
        public async override Task<Result<UserDTO>> AddAsync(AccountCreateDTO entityDTO, CancellationToken cancellationToken = default)
        {
            try
            {
                Expression<Func<User, bool>> filters = u =>
                (entityDTO.Email == null || u.Email == entityDTO.Email) &&
                (u.PhoneNumber == $"{entityDTO.PhoneNumberCode}{entityDTO.PhoneNumber}");
                var sameEmailUser = await Repository.GetAll().FirstOrDefaultAsync(filters, cancellationToken: cancellationToken);
                if (sameEmailUser is not null)
                {
                    return Result<UserDTO>.Failure("This Email or Phone is already registered. Please Sign In.");
                }
                var newUser = entityDTO.Adapt<User>();
                newUser.CreatedBy = "System";
                newUser.CreatedDate = DateTime.Now;
                newUser.UpdatedBy = "System";
                newUser.UpdatedDate = DateTime.Now;
                newUser.IsActive = true;
                newUser.PhoneNumbers?.Add(new PhoneNumber 
                {
                    Number = entityDTO.PhoneNumber,
                    Code = entityDTO.PhoneNumberCode,
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                });
                
                //Assign default client role to the new user
                var clientRole = await roleManager.FindByNameAsync("Client");
                if (clientRole is null)
                {
                    clientRole = new Role { Name = "Client", CreatedBy = "System", CreatedDate = DateTime.Now, UpdatedBy = "System", UpdatedDate = DateTime.Now, IsActive = true };
                    await roleManager.CreateAsync(clientRole);
                }
                
                newUser.UserRoles?.Add(new UserRole
                {
                    Role = clientRole,
                    RoleId = clientRole.Id,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                });
                //await Repository.AddAsync(newUser, cancellationToken);
                //await userManager.AddToRoleAsync(newUser, clientRole.Name!);
                var createResult = await userManager.CreateAsync(newUser, entityDTO.Password);
                if (createResult.Succeeded)
                {
                //await UnitOfWork.SaveChangesAsync(cancellationToken);
                //await base.AddAsync(entityDTO, cancellationToken);
                    return Result<UserDTO>.Success(newUser.Adapt<UserDTO>());
                }
                else
                {
                    var errors = createResult.Errors.Select(e => e.Description).ToArray();
                    return Result<UserDTO>.Failure(errors);
                }
            }
            catch (Exception ex)
            {
                return Result<UserDTO>.Failure(ex.Message);
            }
        }
    }
}
