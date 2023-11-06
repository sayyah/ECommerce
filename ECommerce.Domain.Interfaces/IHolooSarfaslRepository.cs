using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Domain.Interfaces;

public interface IHolooSarfaslRepository : IHolooRepository<HolooSarfasl>
{
    Task<string> Add(string username, CancellationToken cancellationToken);
}
