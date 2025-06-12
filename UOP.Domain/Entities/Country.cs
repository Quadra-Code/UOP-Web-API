using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOP.Domain.Interfaces;

namespace UOP.Domain.Entities
{
    public partial class Country : IAuditEntity<Guid>
    {
        [NotMapped]
        public Guid Id { get => CountryId; set => CountryId = value; }

        public Guid CountryId { get; set; }

        public string Name_En { get; set; } = null!;

        public string Name_Ar { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string Order { get; set; }

        public bool IsActive { get; set; }

        public string Currency { get; set; } = null!;

        public string FlagImage { get; set; } = null!;

        public string FlagThumbnail { get; set; } = null!;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<State> States { get; set; } = [];

        //public virtual ICollection<Engagement> Engagements { get; set; } = [];

        //public virtual ICollection<RegionCountry> RegionCountries { get; set; } = [];
    }
}
