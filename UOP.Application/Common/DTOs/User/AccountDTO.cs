using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOP.Domain.Entities;

namespace UOP.Application.Common.DTOs.User
{
    public class AccountLoginDTO
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class AccountCreateDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumberCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public virtual ICollection<UserRole>? UserRoles { get; set; } = [];
        public virtual ICollection<PhoneNumber>? PhoneNumbers { get; set; } = [];
    }

    public class CreateAdminDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid RoleId { get; set; }
    }
}
