namespace ECommerce.Infrastructure.Repository;

public class UnitRepository(SunflowerECommerceDbContext context) : AsyncRepository<Unit>(context), IUnitRepository
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

    public async Task<Unit> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Units.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> AddAll(IEnumerable<Unit> units, CancellationToken cancellationToken)
    {
        await context.Units.AddRangeAsync(units, cancellationToken);
        return await context.SaveChangesAsync(cancellationToken);
    }

    public int? GetId(int? unitCode, CancellationToken cancellationToken)
    {
        var unit = context.Units.FirstOrDefaultAsync(x => x.UnitCode == unitCode, cancellationToken);
        return unit?.Id;
    }
}
