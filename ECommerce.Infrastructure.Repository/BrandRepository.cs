namespace ECommerce.Infrastructure.Repository;

public class BrandRepository(SunflowerECommerceDbContext context) : AsyncRepository<Brand>(context), IBrandRepository
{
    public async Task<Brand> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Brands.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Brand>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Brand>.ToPagedList(
            await context.Brands.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
