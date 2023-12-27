using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Domain.Interfaces;

public interface IHolooABailRepository : IHolooRepository<HolooABail>
{
    void Add(List<HolooABail> aBails);

    double GetWithACode(int userCode, string aCode, CancellationToken cancellationToken);
}
