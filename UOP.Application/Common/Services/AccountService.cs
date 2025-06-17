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

        public async Task<Result<UserDTO>> CreateStaffAsync(CreateStaffDTO entityDTO, string creatorName)
        {
            try
            {
                Expression<Func<User, bool>> filters = u =>
                (entityDTO.Email == null || u.Email == entityDTO.Email) &&
                (entityDTO.PhoneNumber == null || u.PhoneNumber == entityDTO.PhoneNumber) &&
                (entityDTO.PhoneNumberCode == null || u.PhoneNumberCode == entityDTO.PhoneNumberCode);

                var sameUser = await Repository.GetAll().Include(u => u.UserRoles)!.ThenInclude(u => u.Role).FirstOrDefaultAsync(filters);
                if (sameUser is not null)
                {
                    // check if the user is client only then add to his roles else failure
                    if (sameUser.UserRoles!.Count == 1 && sameUser.UserRoles.Any(ur => ur.Role!.Name == "Client"))
                    {
                        foreach (var roleId in entityDTO.RoleIds)
                        {
                            sameUser.UserRoles?.Add(new UserRole
                            {
                                RoleId = roleId,
                                CreatedBy = "System",
                                CreatedDate = DateTime.Now,
                                UpdatedBy = "System",
                                UpdatedDate = DateTime.Now,
                            });
                        }
                        sameUser.UpdatedBy = creatorName;
                        sameUser.UpdatedDate = DateTime.Now;
                        await UnitOfWork.SaveChangesAsync();
                        return Result<UserDTO>.Success(sameUser.Adapt<UserDTO>());
                    }
                    return Result<UserDTO>.Failure("Staff with same email or phone number already exists");
                }
                var newStaff = entityDTO.Adapt<User>();
                newStaff.CreatedBy = creatorName;
                newStaff.CreatedDate = DateTime.Now;
                newStaff.UpdatedBy = creatorName;
                newStaff.UpdatedDate = DateTime.Now;
                newStaff.IsActive = true;

                foreach (var roleId in entityDTO.RoleIds)
                {
                    newStaff.UserRoles?.Add(new UserRole
                    {
                        RoleId = roleId,
                        CreatedBy = "System",
                        CreatedDate = DateTime.Now,
                        UpdatedBy = "System",
                        UpdatedDate = DateTime.Now,
                    });
                }

                newStaff.PhoneNumbers?.Add(new PhoneNumber
                {
                    Code = entityDTO.PhoneNumberCode,
                    Number = entityDTO.PhoneNumber,
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                });
                var createResult = await userManager.CreateAsync(newStaff, "Aa123#");
                if (createResult.Succeeded)
                {
                    return Result<UserDTO>.Success(newStaff.Adapt<UserDTO>());
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

        // Login client account
        public async Task<Result<UserDTO>> LoginAsync(AccountLoginDTO accountLoginDTO)
        {
            try
            {

                //if (accountLoginDTO.PhoneNumberCode == "+20")
                //{
                //    accountLoginDTO.PhoneNumberCode = "+2";
                //    accountLoginDTO.PhoneNumber = $"0{entityDTO.PhoneNumber}";
                //}


                Expression<Func<User, bool>> filters = u =>
                    u.Email == accountLoginDTO.EmailOrPhone ||
                    u.PhoneNumber == (u.PhoneNumberCode!.StartsWith("+20") ? accountLoginDTO.EmailOrPhone.Substring(2) : accountLoginDTO.EmailOrPhone) &&
                    u.UserRoles!.Any(ur => ur.Role!.Name == "Client");

                var user = await Repository.GetAll().FirstOrDefaultAsync(filters);
                
                var fakePasswordCheck = Task.Delay(500); // Simulate password check delay  

                if (user is not null)
                {
                    var passwordCheck = userManager.CheckPasswordAsync(user, accountLoginDTO.Password);
                    await Task.WhenAll(fakePasswordCheck, passwordCheck);

                    if (passwordCheck.Result)
                    {
                        return Result<UserDTO>.Success(user.Adapt<UserDTO>());
                    }
                }
                else
                {
                    await fakePasswordCheck; // Ensure consistent delay even if user is not found  
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
                (u.PhoneNumberCode == entityDTO.PhoneNumberCode && u.PhoneNumber == entityDTO.PhoneNumber);
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
                var createResult = await userManager.CreateAsync(newUser, entityDTO.Password);
                if (createResult.Succeeded)
                {
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
