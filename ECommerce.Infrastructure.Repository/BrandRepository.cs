using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class BrandRepository : AsyncRepository<Brand>, IBrandRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public BrandRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Brand> GetByName(string name, CancellationToken cancellationToken)
    {
        return await _context.Brands.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Brand>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Brand>.ToPagedList(
            await _context.Brands.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
