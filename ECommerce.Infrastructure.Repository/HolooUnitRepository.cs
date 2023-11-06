using ECommerce.Domain.Entities.HolooEntity;
<<<<<<< HEAD
using ECommerce.Infrastructure.DataContext;
=======
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

namespace ECommerce.Infrastructure.Repository;

public class HolooUnitRepository : HolooRepository<HolooUnit>, IHolooUnitRepository
{
    private readonly HolooDbContext _context;

    public HolooUnitRepository(HolooDbContext context) : base(context)
    {
        _context = context;
    }
}
