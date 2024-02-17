using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogCategories.Results;

namespace ECommerce.Application.Services.BlogCategories.Commands;

    public class CreateBlogCategoryCommand: ICommand<BlogCategoryResult>
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int? Depth { get; set; } = 0;

        public string? Description { get; set; }

        //ForeignKey
        public int? ParentId { get; set; }

        public ICollection<int>? BlogCategoriesId { get; set; } = new List<int>();

        public ICollection<int>? BlogsId { get; set; }
    }
