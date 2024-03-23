using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class SizeRepository(SunflowerECommerceDbContext context) : RepositoryBase<Size>(context), ISizeRepository
{
    public async Task<Size> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Sizes.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public PagedList<Size> Search(PaginationParameters paginationParameters)
    {
        return PagedList<Size>.ToPagedList(
           context.Sizes.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
