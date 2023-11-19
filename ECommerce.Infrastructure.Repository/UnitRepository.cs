using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;
using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class UnitRepository : AsyncRepository<Unit>, IUnitRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public UnitRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PagedList<Unit>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Unit>.ToPagedList(
            await _context.Units.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }

    public async Task<Unit> GetByName(string name, CancellationToken cancellationToken)
    {
        return await _context.Units.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> AddAll(IEnumerable<Unit> units, CancellationToken cancellationToken)
    {
        await _context.Units.AddRangeAsync(units, cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public int? GetId(int? unitCode, CancellationToken cancellationToken)
    {
        var unit = _context.Units.FirstOrDefaultAsync(x => x.UnitCode == unitCode, cancellationToken);
        return unit?.Id;
    }
}
