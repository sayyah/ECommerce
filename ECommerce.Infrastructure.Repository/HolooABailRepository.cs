using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Infrastructure.Repository;

public class HolooABailRepository(HolooDbContext context) : HolooRepository<HolooABail>(context), IHolooABailRepository
{
    public double GetWithACode(int userCode, string aCode, CancellationToken cancellationToken)
    {
        var result = (from d in context.ABAILPRE.Where(c => c.A_Code == aCode)
                      join dr in context.FBAILPRE.Where(c => c.UserCode == userCode) on d.Fac_Code equals dr.Fac_Code
                      select d.First_Article).Sum();

        return result;
    }

    public async Task<bool> Add(List<HolooABail> aBails, CancellationToken cancellationToken)
    {
        await context.AddRangeAsync(aBails, cancellationToken);
        var result = await context.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    public async Task<List<HolooABail>> GetAll(CancellationToken cancellationToken)
    {
        return await context.ABAILPRE.ToListAsync(cancellationToken);
    }
}
