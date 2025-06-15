using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOP.Application.Common.DTOs.User
{
    public class LoginResponseDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public string FullName { get; set; }
        public string? PictureUrl { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
    }
}
