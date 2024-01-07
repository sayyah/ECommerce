namespace ECommerce.Infrastructure.Repository;

public class SizeRepository(SunflowerECommerceDbContext context) : AsyncRepository<Size>(context), ISizeRepository
{
    public async Task<Size> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Sizes.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Size>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Size>.ToPagedList(
            await context.Sizes.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
