using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class StoreRepository(SunflowerECommerceDbContext context) : RepositoryBase<Store>(context), IStoreRepository
{
    public async Task<Store> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Stores.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public PagedList<Store> Search(PaginationParameters paginationParameters)
    {
        return PagedList<Store>.ToPagedList(
            context.Stores.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
