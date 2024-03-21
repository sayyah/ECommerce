using ECommerce.Domain.Entities.HolooEntity;
using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class HolooSGroupRepository(HolooDbContext context) : HolooRepository<HolooSGroup>(context),
    IHolooSGroupRepository
{
    public async Task<IEnumerable<HolooSGroup>> GetSGroupByMCode(string mCode, CancellationToken cancellationToken)
    {
        return await context.S_GROUP.Where(x => x.M_groupcode == mCode).ToListAsync(cancellationToken);
    }
}
