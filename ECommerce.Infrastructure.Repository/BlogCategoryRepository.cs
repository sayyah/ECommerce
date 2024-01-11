namespace ECommerce.Infrastructure.Repository;

public class BlogCategoryRepository(SunflowerECommerceDbContext context) : RepositoryBase<BlogCategory>(context),
    IBlogCategoryRepository
{
    public async Task<BlogCategory?> GetByName(string name, int? parentId, CancellationToken cancellationToken)
    {
        return await context.BlogCategories.Where(x => x.Name == name && x.Parent!.Id == parentId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<CategoryParentViewModel>?> Parents(int blogId, CancellationToken cancellationToken)
    {
        var blogCategoryId = 0;
        if (blogId > 0)
        {
            var temp = context.Blogs.Where(x => x.Id == blogId).Include(x => x.BlogCategory);
            var blog = await temp.FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (blog != null) blogCategoryId = blog.BlogCategoryId;
        }

        var allCategory = await context.BlogCategories.ToListAsync(cancellationToken);

        var result = await Children(allCategory, blogCategoryId, null);
        return result.OrderBy(x => x.DisplayOrder).ToList();
    }

    public async Task<PagedList<BlogCategory>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<BlogCategory>.ToPagedList(
            await context.BlogCategories.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }

    private async Task<List<CategoryParentViewModel>> Children(List<BlogCategory> allCategory,
        int blogCategoryId, int? parentId)
    {
        var temp = new List<CategoryParentViewModel>();
        var ret = new List<CategoryParentViewModel>();
        foreach (var parent in allCategory.Where(p => p.ParentId == parentId).ToList())
        {
            if (allCategory.Any(p => p.ParentId == parent.Id))
                temp = await Children(allCategory, blogCategoryId, parent.Id);

            ret.Add(new CategoryParentViewModel
            {
                Id = parent.Id,
                Name = parent.Name,
                Depth = (int)parent.Depth,
                Children = temp,
                Checked = blogCategoryId == parent.Id
            });
            temp = new List<CategoryParentViewModel>();
        }

        return ret;
    }
}
