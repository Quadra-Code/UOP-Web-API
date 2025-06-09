using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOP.Domain.Interfaces;

namespace UOP.Domain.Entities
{
    public partial class Permission : IAuditEntity<Guid>
    {
        [NotMapped]
        public Guid Id { get => PermissionId;  set => PermissionId = value; }

        public Guid PermissionId { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public string Name { get; set; } = null!;

        public Guid? PermissionTypeId { get; set; }

        public string Order { get; set; }

        //public Guid? ApplicationEntityId { get; set; }

        //public Guid OrganizationId { get; set; }

        public virtual PermissionType? PermissionType { get; set; }

        //public virtual ApplicationEntity? ApplicationEntity { get; set; }

        //public virtual ICollection<RoleItemPermission> RoleItemPermissions { get; set; } = new List<RoleItemPermission>();
    }
}
