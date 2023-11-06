<<<<<<< HEAD
﻿using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;
=======
﻿namespace ECommerce.Infrastructure.Repository;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

public class KeywordRepository : AsyncRepository<Keyword>, IKeywordRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public KeywordRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Keyword> GetByKeywordText(string keywordText, CancellationToken cancellationToken)
    {
        return await _context.Keywords.Where(x => x.KeywordText == keywordText)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> AddAll(IEnumerable<Keyword> keywords, CancellationToken cancellationToken)
    {
        await _context.Keywords.AddRangeAsync(keywords, cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken);
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
