using ECommerce.Domain.Entities.HolooEntity;
using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class HolooSanadListRepository(HolooDbContext context) : HolooRepository<HolooSndList>(context),
    IHolooSanadListRepository
{
    public void Add(HolooSndList sanadList)
    {
        context.Snd_List.Add(sanadList);
    }
}
