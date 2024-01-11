namespace ECommerce.Infrastructure.Repository;

public class KeywordRepository(SunflowerECommerceDbContext context) : RepositoryBase<Keyword>(context),
    IKeywordRepository
{
    public async Task<Keyword?> GetByKeywordText(string keywordText, CancellationToken cancellationToken)
    {
        return await context.Keywords.Where(x => x.KeywordText == keywordText)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void AddAll(IEnumerable<Keyword> keywords)
    {
        context.Keywords.AddRange(keywords);
    }

    public async Task<List<Keyword>> GetByProductId(int productId, CancellationToken cancellationToken)
    {
        return await context.Keywords.Where(x => x.Products.Any(y => y.Id == productId))
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedList<Keyword>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Keyword>.ToPagedList(
            await context.Keywords.Where(x => x.KeywordText.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
