using ECommerce.Domain.Entities.HolooEntity;
using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class HolooUnitRepository(HolooDbContext context) : HolooRepository<HolooUnit>(context), IHolooUnitRepository
{
    private readonly HolooDbContext context = context;
}
