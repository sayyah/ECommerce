using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Domain.Interfaces;

public interface IHolooMGroupRepository : IHolooRepository<HolooMGroup>
{
    Task<HolooMGroup> GetByCode(string code, CancellationToken cancellationToken);
}
