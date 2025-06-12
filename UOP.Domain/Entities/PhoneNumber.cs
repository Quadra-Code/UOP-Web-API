using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOP.Domain.Interfaces;

namespace UOP.Domain.Entities
{
    public class PhoneNumber : IAuditEntity<Guid>
    {
        public Guid Id { get => PhoneNumberId; set => PhoneNumberId = value; }
        public Guid PhoneNumberId { get; set; }
        public Guid UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public string Number { get; set; }
        public string Code { get; set; }
        public virtual User? User { get; set; }
    }
}
