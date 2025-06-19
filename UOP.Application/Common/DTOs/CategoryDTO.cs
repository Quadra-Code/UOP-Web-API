namespace UOP.Application.Common.DTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name_Ar { get; set; }

        public string Name_En { get; set; }

        public string Order { get; set; }

        public Guid? ParentId { get; set; }

        public string? ParentName_Ar { get; set; }

        public string? ParentName_En { get; set; }

        public string? Description_Ar { get; set; }
        
        public string? Description_En { get; set; }

        public bool IsParent { get; set; } = false;

        public string? IconUrl { get; set; }
    }

    public class CreateCategoryDTO
    {
        public string Name_Ar { get; set; }

        public string Name_En { get; set; }

        public string Order { get; set; }

        public Guid? ParentId { get; set; }

        public string? Description_Ar { get; set; }

        public string? Description_En { get; set; }

        public bool IsParent { get; set; } = false;

        public string? IconUrl { get; set; }
    }
}
