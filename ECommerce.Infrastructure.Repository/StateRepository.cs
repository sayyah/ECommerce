namespace ECommerce.Infrastructure.Repository;

public class StateRepository(SunflowerECommerceDbContext context) : AsyncRepository<State>(context), IStateRepository
{
    public async Task<State> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.States.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<State>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<State>.ToPagedList(
            await context.States.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Name).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
