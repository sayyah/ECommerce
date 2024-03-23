using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class SupplierRepository(SunflowerECommerceDbContext context) : RepositoryBase<Supplier>(context),
    ISupplierRepository
{
    public async Task<Supplier> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Suppliers.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public PagedList<Supplier> Search(PaginationParameters paginationParameters)
    {
        return PagedList<Supplier>.ToPagedList(
           context.Suppliers.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
