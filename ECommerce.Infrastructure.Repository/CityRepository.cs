namespace ECommerce.Infrastructure.Repository;

public class CityRepository(SunflowerECommerceDbContext context) : AsyncRepository<City>(context), ICityRepository
{
    public async Task<City> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Cities.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<City>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<City>.ToPagedList(
            await context.Cities.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Name).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
