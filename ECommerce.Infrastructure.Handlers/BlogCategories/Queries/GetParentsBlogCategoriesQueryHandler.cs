using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogCategories.Queries;
using ECommerce.Application.Services.BlogCategories.Results;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.BlogCategories.Queries;

public class GetParentsBlogCategoriesQueryHandler(IUnitOfWork unitOfWork) :
        IQueryHandler<GetBlogParentCategoryByIdQuery, List<BLogCategoryParentResult>>
{
    private readonly IBlogCategoryRepository _blogCategoryRepository = unitOfWork.GetRepository<BlogCategoryRepository, BlogCategory>();

    public async Task<List<BLogCategoryParentResult>> HandleAsync(GetBlogParentCategoryByIdQuery query)
    {
        var blogCategories = _blogCategoryRepository.Parents(query.Id, CancellationToken.None) ?? throw new Exception();

        var result = (await Children(blogCategories.Result!, query.Id, null)).OrderBy(x => x.DisplayOrder);
        return result.ToList();
    }

    private async Task<List<BLogCategoryParentResult>> Children(List<BlogCategory> allCategory,
        int blogCategoryId, int? parentId)
    {
        var temp = new List<BLogCategoryParentResult>();
        var ret = new List<BLogCategoryParentResult>();
        foreach (var parent in allCategory.Where(p => p.ParentId == parentId).ToList())
        {
            if (allCategory.Any(p => p.ParentId == parent.Id))
                temp = await Children(allCategory, blogCategoryId, parent.Id);

            ret.Add(new BLogCategoryParentResult
            {
                Id = parent.Id,
                Name = parent.Name,
                Depth = (int)parent.Depth,
                Children = temp,
                Checked = blogCategoryId == parent.Id
            });
            temp = new List<BLogCategoryParentResult>();
        }
        return ret;
    }
}
