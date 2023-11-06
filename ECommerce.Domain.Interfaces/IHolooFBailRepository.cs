using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Domain.Interfaces;

public interface IHolooFBailRepository : IHolooRepository<HolooFBail>
{
    Task<string> Add(HolooFBail bail, CancellationToken cancellationToken);
    Task<(string fCode, int fCodeC)> GetFactorCode(CancellationToken cancellationToken);
}
