using ECommerce.Application.Base.Services.Interfaces;

namespace ECommerce.Application.Services.BlogCategories.Commands;

    public class EditBlogCategoryCommand : ICommand<bool>
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
        public EditBlogCategoryCommand? Parent { get; set; }
        public ICollection<int>? BlogsId { get; set; }
    }
