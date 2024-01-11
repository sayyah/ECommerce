using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Domain.Interfaces;

public interface IHolooSanadListRepository : IHolooRepository<HolooSndList>
{
    void Add(HolooSndList sanadList);
}
