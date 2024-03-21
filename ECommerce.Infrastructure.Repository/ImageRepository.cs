using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class ImageRepository(SunflowerECommerceDbContext context) : RepositoryBase<Image>(context), IImageRepository
{
    public async Task DeleteByName(string name, CancellationToken cancellationToken)
    {
        var image = await context.Images.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
        context.Images.Remove(image);
    }

    public async Task<List<Image>?> GetByProductId(int productId, CancellationToken cancellationToken)
    {
        return await
            context.Images.Where(x => x.ProductId == productId).ToListAsync(cancellationToken);
    }

    public async Task<Image?> GetByBlogId(int blogId, CancellationToken cancellationToken)
    {
        return await context.Images.Where(x => x.BlogId == blogId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Image>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Image>.ToPagedList(
            await context.Images.Where(x => x.Alt.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
