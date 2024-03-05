using ECommerce.Application.Base.Services.Interfaces;

namespace ECommerce.Application.Services.BlogCategories.Commands;

    public class DeleteBlogCategoryCommand : ICommand<bool>
    {
        public int Id { get; set; }
    }
