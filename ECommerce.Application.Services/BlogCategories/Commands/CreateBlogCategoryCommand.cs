using ECommerce.Application.Base.Services.Interfaces;

namespace ECommerce.Application.Services.BlogCategories.Commands;

    public class CreateBlogCategoryCommand: ICommand<Boolean>
    {
        public string? Name { get; set; }
        public int? CreatorUserId { get; set; }
        public int? EditorUserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public int? Depth { get; set; } = 0;
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public ICollection<int>? BlogsId { get; set; }
    }
