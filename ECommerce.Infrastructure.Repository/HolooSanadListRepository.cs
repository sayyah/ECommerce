using ECommerce.Domain.Entities.HolooEntity;
using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class HolooSanadListRepository : HolooRepository<HolooSndList>, IHolooSanadListRepository
{
    private readonly HolooDbContext _context;

    public HolooSanadListRepository(HolooDbContext context) : base(context)
    {
        _context = context;
    }

    public void Add(HolooSndList sanadList)
    {
        _context.Snd_List.Add(sanadList);
    }
}
