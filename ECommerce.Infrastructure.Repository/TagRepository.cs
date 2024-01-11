using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class TagRepository(SunflowerECommerceDbContext context) : RepositoryBase<Tag>(context), ITagRepository
{
    public async Task<Tag> GetByTagText(string tagText, CancellationToken cancellationToken)
    {
        return await context.Tags.Where(x => x.TagText == tagText).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<TagProductId>> GetByProductId(int productId, CancellationToken cancellationToken)
    {
        return await context.Tags.Where(x => x.Products.Any(y => y.Id == productId))
            .Select(x => new TagProductId { Id = x.Id, ProductsId = x.Products.Select(x => x.Id) })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Tag>> GetAllProductTags(CancellationToken cancellationToken)
    {
        var result = await context.Tags.Where(x => x.Products.Any()).ToListAsync(cancellationToken);
        return result;
    }

    public async Task<List<Tag>> GetAllBlogTags(CancellationToken cancellationToken)
    {
        var result = await context.Tags.Where(x => x.Blogs.Any()).ToListAsync(cancellationToken);
        return result;
    }

    public PagedList<Tag> Search(PaginationParameters paginationParameters)
    {
        return PagedList<Tag>.ToPagedList(
            context.Tags.Where(x => x.TagText.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
