using ECommerce.Domain.Entities.HolooEntity;
<<<<<<< HEAD
using ECommerce.Infrastructure.DataContext;
=======
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

namespace ECommerce.Infrastructure.Repository;

public class HolooSGroupRepository : HolooRepository<HolooSGroup>, IHolooSGroupRepository
{
    private readonly HolooDbContext _context;

    public HolooSGroupRepository(HolooDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HolooSGroup>> GetSGroupByMCode(string mCode, CancellationToken cancellationToken)
    {
        return await _context.S_GROUP.Where(x => x.M_groupcode == mCode).ToListAsync(cancellationToken);
    }
}
