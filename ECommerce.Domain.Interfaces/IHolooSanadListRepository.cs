using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Domain.Interfaces;

public interface IHolooSanadListRepository : IHolooRepository<HolooSndList>
{
    Task<bool> Add(HolooSndList sanadList, CancellationToken cancellationToken);
}
