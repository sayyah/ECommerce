using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class ColorRepository(SunflowerECommerceDbContext context) : RepositoryBase<Color>(context), IColorRepository
{
    public async Task<Color?> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Colors.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Color>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Color>.ToPagedList(
            await context.Colors.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
