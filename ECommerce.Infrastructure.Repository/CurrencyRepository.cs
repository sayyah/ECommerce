using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class CurrencyRepository(SunflowerECommerceDbContext context) : RepositoryBase<Currency>(context),
    ICurrencyRepository
{
    public async Task<Currency?> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Currencies.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public PagedList<Currency> Search(PaginationParameters paginationParameters)
    {
        return PagedList<Currency>.ToPagedList(
            context.Currencies.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
