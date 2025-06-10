using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOP.Domain.Interfaces;

namespace UOP.Domain.Entities
{
    public partial class State : IAuditEntity<Guid>
    {
        [NotMapped]
        public Guid Id { get => StateId; set => StateId = value; }

        public Guid StateId { get; set; }

        public Guid CountryId { get; set; }

        public string Name_En { get; set; } = null!;

        public string Name_Ar { get; set; } = null!;

        public bool IsActive { get; set; }

        public string Order { get; set; }

        public string MapUrl { get; set; } = null!;

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Country Country { get; set; } = null!;

        public virtual ICollection<City> Cities { get; set; } = new List<City>();
    }
}
