using ECommerce.Domain.Entities.HolooEntity;
using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class HolooMGroupRepository(HolooDbContext context) : HolooRepository<HolooMGroup>(context),
    IHolooMGroupRepository
{
    public async Task<HolooMGroup> GetByCode(string code, CancellationToken cancellationToken)
    {
        return await context.M_GROUP.Where(x => x.M_groupcode == code).FirstAsync(cancellationToken);
    }
}
