using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using UOP.Domain.Extensions;
using UOP.Domain.Interfaces;

namespace UOP.Domain.Entities
{
    public class User : IdentityUser<Guid>, IAuditEntity<Guid>
    {
        public User() => UserId = UuidV7.NewGuid();

        [NotMapped]
        public override Guid Id { get => UserId; set => UserId = value; }

        public Guid UserId { get; set; }

        public override string UserName { get; set; } = null!;
        public string? FullName { get; set; }

        public override string? Email { get; set; }

        public string PhoneNumberCode { get; set; }

        public override string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public bool IsStaff { get; set; }

        public string? PictureUrl { get; set; }

        public string? CoverPhotoUrl { get; set; }

        public string? UserPictureUrl { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<UserRole>? UserRoles { get; set; } = [];
        public virtual ICollection<PhoneNumber>? PhoneNumbers { get; set; } = [];
        public virtual ICollection<Address>? Addresses { get; set; } = [];
    }
}
