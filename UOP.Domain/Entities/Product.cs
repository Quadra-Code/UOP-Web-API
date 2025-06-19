using System.ComponentModel.DataAnnotations.Schema;
using UOP.Domain.Interfaces;

namespace UOP.Domain.Entities
{
    public class Product : IAuditEntity<Guid>
    {
        [NotMapped]
        public Guid Id { get => ProductId; set => ProductId = value; }

        public Guid ProductId { get; set; }
        
        public Guid? CategoryId { get; set; }
        
        public string CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public string? UpdatedBy { get; set; }
        
        public DateTime? UpdatedDate { get; set; }
        
        public bool IsDeleted { get; set; }

        public string Name_Ar { get; set; }

        public string Name_En { get; set; }

        public string Order { get; set; }
        
        public string? Description { get; set; }
        
        public bool IsActive { get; set; }

        public virtual Category Category { get; set; } = null!;
    }
}
