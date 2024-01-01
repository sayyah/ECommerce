using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Infrastructure.Repository;

public class HolooSanadRepository : HolooRepository<HolooSanad>, IHolooSanadRepository
{
    private readonly HolooDbContext _context;

    public HolooSanadRepository(HolooDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(string, string)> Add(HolooSanad sanad, CancellationToken cancellationToken)
    {
        var sanadCodeCustomer = await _context.Sanad.OrderByDescending(s => s.Sanad_Code_C).Select(c => c.Sanad_Code_C)
            .FirstAsync(cancellationToken);
        sanad.Sanad_Code_C = sanadCodeCustomer ?? 1;
        _context.Sanad.Add(sanad);
        return (sanad.Sanad_Code.ToString(), sanad.Sanad_Code_C.ToString());
    }
}
