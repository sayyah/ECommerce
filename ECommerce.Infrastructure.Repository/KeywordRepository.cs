namespace ECommerce.Infrastructure.Repository;

public class KeywordRepository : RepositoryBase<Keyword>, IKeywordRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public KeywordRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Keyword?> GetByKeywordText(string keywordText, CancellationToken cancellationToken)
    {
        return await _context.Keywords.Where(x => x.KeywordText == keywordText)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void AddAll(IEnumerable<Keyword> keywords)
    {
        _context.Keywords.AddRange(keywords);
    }

    public async Task<List<Keyword>> GetByProductId(int productId, CancellationToken cancellationToken)
    {
        return await _context.Keywords.Where(x => x.Products.Any(y => y.Id == productId))
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedList<Keyword>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Keyword>.ToPagedList(
            await _context.Keywords.Where(x => x.KeywordText.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
