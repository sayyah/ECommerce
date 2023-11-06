using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Domain.Interfaces;

public interface IHolooABailRepository : IHolooRepository<HolooABail>
{
    Task<bool> Add(List<HolooABail> aBails, CancellationToken cancellationToken);

    Task<List<HolooABail>> GetAll(CancellationToken cancellationToken);

    double GetWithACode(int userCode, string aCode, CancellationToken cancellationToken);
}
