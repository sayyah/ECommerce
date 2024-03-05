using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogCategories.Queries;
using ECommerce.Application.Services.BlogCategories.Results;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.BlogCategories.Queries;

    public class GetByIdBlogCategoriesQueryHandler(IUnitOfWork unitOfWork):
        IQueryHandler<GetBlogCategoryByIdQuery, BlogCategoryResult>
    {
        private readonly IBlogCategoryRepository _blogCategoryRepository =unitOfWork.GetRepository<BlogCategoryRepository, BlogCategory>();

        public async Task<BlogCategoryResult> HandleAsync(GetBlogCategoryByIdQuery query)
        {
            var blogCategory = _blogCategoryRepository.GetByIdWithInclude("Blogs,BlogCategories,Parent", query.Id) ?? throw new Exception();
            var result = new BlogCategoryResult
            {
                Id = blogCategory.Id,
                CreatorUserId = blogCategory.CreatorUserId,
                EditorUserId = blogCategory.EditorUserId,
                CreatedDate = blogCategory.CreatedDate,
                UpdatedDate = blogCategory.UpdatedDate,
                Name = blogCategory.Name,
                Depth = blogCategory.Depth,
                Description = blogCategory.Description,
                ParentId = blogCategory.ParentId,
                Parent = blogCategory.Parent,
                BlogCategories = blogCategory.BlogCategories != null ? blogCategory.BlogCategories!.ToList() : null,
                Blogs = blogCategory.Blogs != null ? blogCategory.Blogs!.ToList() : null,
            };
            return result;
        }
    }
