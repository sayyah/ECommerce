using ECommerce.API.DataTransferObject.Blogs;

namespace ECommerce.API.DataTransferObject.BlogCategories.Commands;

    public class DeleteBlogCategoryDto : IBlogDto
    {
        public int Id { get; set; }
    }
