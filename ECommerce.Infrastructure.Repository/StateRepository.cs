using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class StateRepository(SunflowerECommerceDbContext context) : RepositoryBase<State>(context), IStateRepository
{
    public async Task<State> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.States.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public PagedList<State> Search(PaginationParameters paginationParameters)
    {
        return PagedList<State>.ToPagedList(
            context.States.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Name),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
