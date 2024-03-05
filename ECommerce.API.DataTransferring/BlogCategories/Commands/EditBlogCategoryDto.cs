
namespace ECommerce.API.DataTransferObject.BlogCategories.Commands;

    public class EditBlogCategoryDto : IBlogCategoryDto
    {
        public int Id { get; set; }
        public int? CreatorUserId { get; set; }
        public int? EditorUserId { get; set; } 
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public string? Name { get; set; }
        public int? Depth { get; set; } = 0;
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public ICollection<int>? BlogsId { get; set; }
}

