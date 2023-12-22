using ECommerce.Domain.Entities.HolooEntity;

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

    public void Add(List<HolooABail> aBails)
    {
        _context.AddRangeAsync(aBails);
    }
}
