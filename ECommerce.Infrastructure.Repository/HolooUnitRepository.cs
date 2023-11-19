using ECommerce.Domain.Entities.HolooEntity;
using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class HolooUnitRepository : HolooRepository<HolooUnit>, IHolooUnitRepository
{
    private readonly HolooDbContext _context;

    public HolooUnitRepository(HolooDbContext context) : base(context)
    {
        _context = context;
    }
}
