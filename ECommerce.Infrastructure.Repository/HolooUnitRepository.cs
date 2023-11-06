using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Infrastructure.Repository;

public class HolooUnitRepository : HolooRepository<HolooUnit>, IHolooUnitRepository
{
    private readonly HolooDbContext _context;

    public HolooUnitRepository(HolooDbContext context) : base(context)
    {
        _context = context;
    }
}
