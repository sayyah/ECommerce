using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class StoreRepository(SunflowerECommerceDbContext context) : RepositoryBase<Store>(context), IStoreRepository
{
    public async Task<Store> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Stores.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Store>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Store>.ToPagedList(
            await context.Stores.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
