using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Domain.Interfaces;

public interface IHolooSanadRepository : IHolooRepository<HolooSanad>
{
    Task<(string, string)> Add(HolooSanad sanad, CancellationToken cancellationToken);
}
