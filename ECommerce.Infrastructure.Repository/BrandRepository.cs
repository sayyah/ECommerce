using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class BrandRepository(SunflowerECommerceDbContext context) : RepositoryBase<Brand>(context), IBrandRepository
{
    public async Task<Brand?> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Brands.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public PagedList<Brand> Search(PaginationParameters paginationParameters)
    {
        return PagedList<Brand>.ToPagedList(
            context.Brands.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
