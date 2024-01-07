using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Infrastructure.Repository;

public class HolooUnitRepository(HolooDbContext context) : HolooRepository<HolooUnit>(context), IHolooUnitRepository
{
    private readonly HolooDbContext _context = context;
}
