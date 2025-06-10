using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOP.Domain.Interfaces;

namespace UOP.Application.Common.DTOs
{
    public class UserDto : IEntity<Guid>
    {
        //this should be mapping for User Entity
        public Guid Id { get { return UserId; } set { UserId = value; } }

        //public string PrimaryKeyName => nameof(UserId);

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public bool IsActive { get; set; }

        public string? PictureUrl { get; set; }

        public string? CoverPhotoUrl { get; set; }

        public string? UserPictureUrl { get; set; }

        public Guid? GenderId { get; set; }

        public Guid IdentitySourceId { get; set; }

        public bool? IsPublicProfile { get; set; }

        public string CreatedBy { get; set; }

        public Guid? EmployeeId { get; set; }

    }
}
