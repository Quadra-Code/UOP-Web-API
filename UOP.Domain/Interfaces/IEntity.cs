using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOP.Domain.Interfaces
{
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
        //string PrimaryKeyName { get; }
    }

    public interface IAuditEntity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }
    }

    public interface IDecisionEntity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public bool? IsVerified { get; set; }
        public string? ApproverComment { get; set; }
        public DateTime? VerificationDate { get; set; }
        public Guid? VerifierId { get; set; }
    }
}
