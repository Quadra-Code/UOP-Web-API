using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOP.Domain.Interfaces;

namespace UOP.Domain.Entities
{
    public class Role : IdentityRole<Guid>, IAuditEntity<Guid>
    {
        public override Guid Id { get => RoleId; set => RoleId = value; }
        public Guid RoleId { get; set; }
        public override string? Name { get; set; }
        public override string? NormalizedName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<UserRole>? RoleUsers { get; set; }
    }
}
