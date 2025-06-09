using System.ComponentModel.DataAnnotations.Schema;
using UOP.Domain.Interfaces;

namespace UOP.Domain.Entities
{
    public class Category : IAuditEntity<Guid>
    {
        [NotMapped]
        public Guid Id { get => CategoryId; set => CategoryId = value; }

        public Guid CategoryId { get; set; }
        
        public string CreatedBy { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public string? UpdatedBy { get; set; }
        
        public DateTime? UpdatedDate { get; set; }
        
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        
        public Guid? ParentId { get; set; }
        
        public string? Description { get; set; }
        
        public bool IsActive { get; set; }
        
        public bool IsParent { get; set; }
        
        public string? IconUrl { get; set; }
        
        public virtual ICollection<Category>? SubCategories { get; set; } = [];
        
        //public virtual ICollection<Product>? Products { get; set; } = [];
    }
}
