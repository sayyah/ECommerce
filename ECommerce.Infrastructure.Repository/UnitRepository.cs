namespace ECommerce.Infrastructure.Repository;

public class UnitRepository(SunflowerECommerceDbContext context) : RepositoryBase<Unit>(context), IUnitRepository
{
    public async Task<PagedList<Unit>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Unit>.ToPagedList(
            await context.Units.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }

    public async Task<Unit?> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Units.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public void AddAll(IEnumerable<Unit> units)
    {
        context.Units.AddRange(units);
    }

    public int? GetId(int? unitCode, CancellationToken cancellationToken)
    {
        var unit = context.Units.FirstOrDefaultAsync(x => x.UnitCode == unitCode, cancellationToken);
        return unit?.Id;
    }
}
