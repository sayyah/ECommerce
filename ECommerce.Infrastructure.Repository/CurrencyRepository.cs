using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class CurrencyRepository(SunflowerECommerceDbContext context) : RepositoryBase<Currency>(context),
    ICurrencyRepository
{
    public async Task<Currency?> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Currencies.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Currency>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Currency>.ToPagedList(
            await context.Currencies.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
