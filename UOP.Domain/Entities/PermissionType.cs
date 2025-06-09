using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOP.Domain.Interfaces;

namespace UOP.Domain.Entities
{
    public partial class PermissionType : IAuditEntity<Guid>
    {
        [NotMapped]
        public Guid Id { get { return PermissionTypeId; } set { PermissionTypeId = value; } }
        public Guid PermissionTypeId { get; set; }

        //public Guid OrganizationId { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
