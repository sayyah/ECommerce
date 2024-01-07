namespace ECommerce.Infrastructure.Repository;

public class SlideShowRepository(SunflowerECommerceDbContext context) : AsyncRepository<SlideShow>(context),
    ISlideShowRepository
{
    public bool IsRepetitiveProduct(int id, int? productId, int? categoryId, CancellationToken cancellationToken)
    {
        var repetitive = true;
        repetitive = id == 0
            ? context.SlideShows.Any(x => x.ProductId == productId)
            : context.SlideShows.Any(x => x.ProductId == productId && x.Id != id);
        return repetitive;
    }

    public async Task<SlideShow> GetByTitle(string title, CancellationToken cancellationToken)
    {
        return await context.SlideShows.Where(x => x.Title == title).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<SlideShow>> GetAllWithInclude(int pageNumber, int pageSize,
        CancellationToken cancellationToken)
    {
        return await context.SlideShows
            .Include(c => c.Category)
            .Include(x => x.Product)
            .ThenInclude(p => p.Prices)
            .ThenInclude(x => x.Discount)
            .OrderBy(x => x.DisplayOrder)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}
