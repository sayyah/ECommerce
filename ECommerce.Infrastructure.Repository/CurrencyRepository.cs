﻿namespace ECommerce.Infrastructure.Repository;

public class CurrencyRepository : AsyncRepository<Currency>, ICurrencyRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public CurrencyRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Currency> GetByName(string name, CancellationToken cancellationToken)
    {
        return await _context.Currencies.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Currency>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Currency>.ToPagedList(
            await _context.Currencies.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}