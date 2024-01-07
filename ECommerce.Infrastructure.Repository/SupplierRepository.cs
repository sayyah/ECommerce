namespace ECommerce.Infrastructure.Repository;

public class SupplierRepository(SunflowerECommerceDbContext context) : AsyncRepository<Supplier>(context),
    ISupplierRepository
{
    public async Task<Supplier> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Suppliers.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Supplier>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Supplier>.ToPagedList(
            await context.Suppliers.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
