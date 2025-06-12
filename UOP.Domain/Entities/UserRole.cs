using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOP.Domain.Interfaces;

namespace UOP.Domain.Entities
{
    public class UserRole : IdentityUserRole<Guid>, IAuditEntity<Guid>
    {
        public Guid Id { get => UserRoleId; set => UserRoleId = value; }
        public Guid UserRoleId { get; set; }
        public Role? Role { get; set; }
        public User? User { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
