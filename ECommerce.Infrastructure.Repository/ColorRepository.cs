using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class ColorRepository(SunflowerECommerceDbContext context) : RepositoryBase<Color>(context), IColorRepository
{
    public async Task<Color?> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Colors.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public  PagedList<Color> Search(PaginationParameters paginationParameters)
    {
        return PagedList<Color>.ToPagedList(
            context.Colors.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
