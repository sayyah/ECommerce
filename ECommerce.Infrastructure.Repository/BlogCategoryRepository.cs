using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class BlogCategoryRepository(SunflowerECommerceDbContext context) : RepositoryBase<BlogCategory>(context),
    IBlogCategoryRepository
{
    public Task<PagedList<BlogCategory>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<BlogCategory?> GetByName(string name, int? parentId, CancellationToken cancellationToken)
    {
        return await context.BlogCategories.Where(x => x.Name == name && x.Parent!.Id == parentId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<BlogCategory>?> Parents(int blogId, CancellationToken cancellationToken)
    {
        var blogCategoryId = 0;
        if (blogId > 0)
        {
            var temp = context.Blogs.Where(x => x.Id == blogId).Include(x => x.BlogCategory);
            var blog = await temp.FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (blog != null) blogCategoryId = blog.BlogCategoryId;
        }

        var allCategory = await context.BlogCategories.ToListAsync(cancellationToken);
        return allCategory;

    }

    public IQueryable<BlogCategory> Search(PaginationParameters paginationParameters)
    {
        return
            context.BlogCategories.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id);
    }


}
