namespace ECommerce.API.DataTransferObject.BlogCategories.Commands;

    public class CreateBlogCategoryDto : IBlogCategoryDto
    {
        public string? Name { get; set; }
        public int? CreatorUserId { get; set; }
        public int? EditorUserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public int? Depth { get; set; } = 0;
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public ICollection<int>? BlogCategoriesId { get; set; }
        public ICollection<int>? BlogsId { get; set; }
    }

