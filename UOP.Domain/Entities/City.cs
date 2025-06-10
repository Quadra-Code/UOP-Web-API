using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOP.Domain.Interfaces;

namespace UOP.Domain.Entities
{
    public partial class City : IAuditEntity<Guid>
    {
        [NotMapped]
        public Guid Id { get => CityId; set => CityId = value; }

        public Guid CityId { get; set; }

        public Guid StateId { get; set; }

        public string Name_En { get; set; } = null!;

        public string Name_Ar { get; set; } = null!;

        public bool IsActive { get; set; }

        public string Order { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public virtual State State { get; set; } = null!;
        //public virtual ICollection<CustomerContact> CustomerContacts { get; set; } = new List<CustomerContact>();
    }
}
