using ECommerce.Domain.Entities.HolooEntity;
using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class HolooABailRepository : HolooRepository<HolooABail>, IHolooABailRepository
{
    private readonly HolooDbContext _context;

    public HolooABailRepository(HolooDbContext context) : base(context)
    {
        _context = context;
    }

    public double GetWithACode(int userCode, string aCode, CancellationToken cancellationToken)
    {
        var result = (from d in _context.ABAILPRE.Where(c => c.A_Code == aCode)
                      join dr in _context.FBAILPRE.Where(c => c.UserCode == userCode) on d.Fac_Code equals dr.Fac_Code
                      select d.First_Article).Sum();

        return result;
    }

    public async Task<bool> Add(List<HolooABail> aBails, CancellationToken cancellationToken)
    {
        await _context.AddRangeAsync(aBails, cancellationToken);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    public async Task<List<HolooABail>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.ABAILPRE.ToListAsync(cancellationToken);
    }
}
