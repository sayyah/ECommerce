using ECommerce.Domain.Entities.HolooEntity;
using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class HolooFBailRepository : HolooRepository<HolooFBail>, IHolooFBailRepository
{
    private readonly HolooDbContext _context;

    public HolooFBailRepository(HolooDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(string fCode, int fCodeC)> GetFactorCode(CancellationToken cancellationToken)
    {
        var holooFBail = await _context.FBAILPRE.OrderByDescending(o => o.Fac_Code)
            .FirstOrDefaultAsync(x => x.Fac_Type.Equals("P"), cancellationToken);
        var fCode = 1;
        var fCodeC = 1;
        if (holooFBail != null)
        {
            fCode = Convert.ToInt32(holooFBail.Fac_Code) + 1;
            fCodeC = Convert.ToInt32(holooFBail.Fac_Code_C) + 1;
        }

        return (fCode.ToString("000000"), fCodeC);
    }

    public async Task<string?> Add(HolooFBail bail, CancellationToken cancellationToken)
    {
        var lastRow = await _context.FBAILPRE.OrderByDescending(o => o.Fac_Code)
            .FirstOrDefaultAsync(x => x.Fac_Type.Equals("P"), cancellationToken);
        var lastFacCode = lastRow == null ? 1 : Convert.ToInt32(lastRow.Fac_Code) + 1;
        bail.Fac_Code_C = lastFacCode;
        bail.Fac_Code = lastFacCode.ToString("000000");
        try
        {
            _context.Add(bail);
            return bail.Fac_Code;
        }
        catch (Exception e)
        {
            lastFacCode += 2;
            bail.Fac_Code_C = lastFacCode;
            bail.Fac_Code = lastFacCode.ToString("000000");
            _context.Add(bail);
            return bail.Fac_Code;
        }
    }
}
