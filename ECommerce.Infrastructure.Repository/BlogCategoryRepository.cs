namespace ECommerce.Infrastructure.Repository;

public class BlogCategoryRepository(SunflowerECommerceDbContext context) : AsyncRepository<BlogCategory>(context),
    IBlogCategoryRepository
{
    public async Task<BlogCategory?> GetByName(string name, int? parentId, CancellationToken cancellationToken)
    {
        return await context.BlogCategories.Where(x => x.Name == name && x.Parent!.Id == parentId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<CategoryParentViewModel>> Parents(int blogId, CancellationToken cancellationToken)
    {
        var blogCategory = 0;
        if (blogId > 0)
        {
            var temp = context.Blogs.Where(x => x.Id == blogId).Include(x => x.BlogCategory);
            blogCategory = temp.First().BlogCategory.Id;
        }

        var allCategory = await context.BlogCategories.ToListAsync(cancellationToken);

        var result = await Children(allCategory, blogCategory, null);
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
        int blogCategory, int? parentId)
    {
        var temp = new List<CategoryParentViewModel>();
        var ret = new List<CategoryParentViewModel>();
        foreach (var parent in allCategory.Where(p => p.ParentId == parentId).ToList())
        {
            if (allCategory.Any(p => p.ParentId == parent.Id))
                temp = await Children(allCategory, blogCategory, parent.Id);

            ret.Add(new CategoryParentViewModel
            {
                Id = parent.Id,
                Name = parent.Name,
                Depth = (int)parent.Depth,
                Children = temp,
                Checked = blogCategory == parent.Id
            });
            temp = new List<CategoryParentViewModel>();
        }

        return ret;
    }
}
