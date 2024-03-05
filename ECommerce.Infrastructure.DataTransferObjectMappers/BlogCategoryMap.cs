using ECommerce.Application.Services.BlogCategories.Commands;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.DataTransferObjectMappers;

    public class BlogCategoryMap : DtoMap
    {
        public BlogCategoryMap()
        {
            CreateMap<CreateBlogCategoryCommand, BlogCategory>().ReverseMap();
            CreateMap<EditBlogCategoryCommand, BlogCategory>().ReverseMap();
        }
    }
