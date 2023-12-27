using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class SupplierRepository : RepositoryBase<Supplier>, ISupplierRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public SupplierRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Supplier?> GetByName(string name, CancellationToken cancellationToken)
    {
        return await _context.Suppliers.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Supplier>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Supplier>.ToPagedList(
            await _context.Suppliers.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
