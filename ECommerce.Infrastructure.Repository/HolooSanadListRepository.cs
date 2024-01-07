using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Infrastructure.Repository;

public class HolooSanadListRepository(HolooDbContext context) : HolooRepository<HolooSndList>(context),
    IHolooSanadListRepository
{
    public async Task<bool> Add(HolooSndList sanadList, CancellationToken cancellationToken)
    {
        // _context.Entry(sanadList).State = EntityState.Detached;
        await context.Snd_List.AddAsync(sanadList, cancellationToken);
        var result = await context.SaveChangesAsync(cancellationToken);
        return result == 0;
    }
}
