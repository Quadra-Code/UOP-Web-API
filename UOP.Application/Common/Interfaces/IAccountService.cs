using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOP.Application.Common.DTOs.User;
using UOP.Domain.Entities;
using UOP.Domain.Interfaces;
using UOP.Domain.Models;

namespace UOP.Application.Interfaces
{
    public interface IAccountService : IGenericService<User, AccountCreateDTO, UserDTO, Guid>
    {
        Task<Result<UserDTO>> CreateStaffAsync(CreateStaffDTO entityDTO, string creatorName);
        Task<Result<UserDTO>> LoginAsync(AccountLoginDTO entityDTO);
    }
}
