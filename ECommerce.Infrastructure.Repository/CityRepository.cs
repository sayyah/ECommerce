using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class CityRepository(SunflowerECommerceDbContext context) : RepositoryBase<City>(context), ICityRepository
{
    public async Task<City?> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Cities.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public PagedList<City> Search(PaginationParameters paginationParameters)
    {
        return PagedList<City>.ToPagedList(
            context.Cities.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Name),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
