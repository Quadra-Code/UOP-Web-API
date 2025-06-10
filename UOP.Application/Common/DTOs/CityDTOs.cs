using System.ComponentModel.DataAnnotations;

namespace UOP.Application.Common.DTOs
{
    public class CityDTO
    {
        public Guid Id { get; set; }

        [Required]
        public string Name_En { get; set; } = null!;

        [Required]
        public string Name_Ar { get; set; } = null!;

        public bool IsActive { get; set; }

        public string Order { get; set; }

        public Guid StateId { get; set; }

        public string? StateName { get; set; }
    }

    public class CreateCityDTO
    {
        [Required]
        public string Name_En { get; set; } = null!;

        [Required]
        public string Name_Ar { get; set; } = null!;

        public bool IsActive { get; set; }

        public string Order { get; set; }

        public Guid StateId { get; set; }
    }
}
