using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class UnitRepository(SunflowerECommerceDbContext context) : RepositoryBase<Unit>(context), IUnitRepository
{
    public PagedList<Unit> Search(PaginationParameters paginationParameters)
    {
        return PagedList<Unit>.ToPagedList(
            context.Units.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id),
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
