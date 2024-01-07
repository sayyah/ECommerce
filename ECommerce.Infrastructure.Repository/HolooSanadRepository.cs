using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Infrastructure.Repository;

public class HolooSanadRepository(HolooDbContext context) : HolooRepository<HolooSanad>(context), IHolooSanadRepository
{
    public async Task<(string, string)> Add(HolooSanad sanad, CancellationToken cancellationToken)
    {
        var sanadCodeCustomer = await context.Sanad.OrderByDescending(s => s.Sanad_Code_C).Select(c => c.Sanad_Code_C)
            .FirstAsync(cancellationToken);
        sanad.Sanad_Code_C = sanadCodeCustomer ?? 1;
        await context.Sanad.AddAsync(sanad, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return (sanad.Sanad_Code.ToString(), sanad.Sanad_Code_C.ToString());
    }
}
