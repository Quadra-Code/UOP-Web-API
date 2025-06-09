using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOP.Domain.Models
{
    public class PermissionDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? PermissionTypeName { get; set; }
        public string Order { get; set; }
        public Guid? PermissionTypeId { get; set; }
        //public Guid? ApplicationEntityId { get; set; }
        //public string? ApplicationEntityName { get; set; }
    }

    public class CreatePermissionDTO
    {
        public string Name { get; set; } = null!;
        public Guid? PermissionTypeId { get; set; }
        public string Order { get; set; }
        //public Guid? ApplicationEntityId { get; set; }
    }
}
