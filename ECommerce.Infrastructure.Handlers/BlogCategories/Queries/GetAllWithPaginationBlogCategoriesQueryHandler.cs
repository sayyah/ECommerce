using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogCategories.Queries;
using ECommerce.Application.Services.BlogCategories.Results;
using ECommerce.Application.Services.Objects;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.BlogCategories.Queries;

public class GetAllWithPaginationBlogCategoriesQueryHandler(IUnitOfWork unitOfWork) :
    IQueryHandler<GetBlogCategoriesQuery, PagedList<BlogCategoryResult>>
{
    private readonly IBlogCategoryRepository _blogCategoryRepository = unitOfWork.GetRepository<BlogCategoryRepository, BlogCategory>();
    public async Task<PagedList<BlogCategoryResult>> HandleAsync(GetBlogCategoriesQuery query)
    {
        IQueryable<BlogCategory> entities = _blogCategoryRepository.Search(query.PaginationParameters);
        PagedList<BlogCategoryResult> pagedList = PagedList<BlogCategoryResult>.ToPagedList(entities.Select(x => new BlogCategoryResult
        {
            Id = x.Id,
            CreatorUserId = x.CreatorUserId,
            EditorUserId = x.EditorUserId,
            CreatedDate = x.CreatedDate,
            UpdatedDate = x.UpdatedDate,
            Name = x.Name,
            Depth = x.Depth,
            Description = x.Description,
            ParentId = x.ParentId,
            Parent = x.Parent,
            BlogCategories = x.BlogCategories.ToList(),
            Blogs = x.Blogs.ToList()
        }),
            query.PaginationParameters.PageNumber,
            query.PaginationParameters.PageSize,
            query.PaginationParameters.Search);

        return pagedList;
    }
}
