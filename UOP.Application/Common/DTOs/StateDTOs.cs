using System.ComponentModel.DataAnnotations;

namespace UOP.Application.Common.DTOs
{
    public class StateDTO
    {
        public Guid Id { get; set; }

        [Required]
        public string Name_En { get; set; } = null!;

        [Required]
        public string Name_Ar { get; set; } = null!;

        public bool IsActive { get; set; }

        public string Order { get; set; }

        public string MapUrl { get; set; } = null!;

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Guid CountryId { get; set; }

        public string? CountryName { get; set; }
    }
    public class CreateStateDTO
    {

        [Required]
        public string Name_En { get; set; } = null!;

        [Required]
        public string Name_Ar { get; set; } = null!;

        public bool IsActive { get; set; }

        public string Order { get; set; }

        public string MapUrl { get; set; } = null!;

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Guid CountryId { get; set; }
    }
}
