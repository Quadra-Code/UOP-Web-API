using System.ComponentModel.DataAnnotations;

namespace UOP.Application.Common.DTOs
{
    public class CountryDTO
    {
        public Guid Id { get; set; }

        [Required]
        public string Name_En { get; set; } = null!;

        [Required]
        public string Name_Ar { get; set; } = null!;

        [Required]
        public string Code { get; set; } = null!;

        public bool IsActive { get; set; }

        [Required]
        public string Currency { get; set; } = null!;

        [Required]
        public string FlagImage { get; set; } = null!;

        [Required]
        public string FlagThumbnail { get; set; } = null!;
    }

    public class CreateCountryDTO
    {
        [Required]
        public string Name_En { get; set; } = null!;

        [Required]
        public string Name_Ar { get; set; } = null!;

        [Required]
        public string Code { get; set; } = null!;

        public bool IsActive { get; set; }

        [Required]
        public string Currency { get; set; } = null!;

        [Required]
        public string FlagImage { get; set; } = null!;

        [Required]
        public string FlagThumbnail { get; set; } = null!;
    }
}
