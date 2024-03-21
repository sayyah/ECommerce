using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class HolooRepository<T>(HolooDbContext context) : IHolooRepository<T>
    where T : class
{
    protected HolooDbContext Context = context;

    public void Dispose()
    {
        Context.Dispose();
    }

    public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken)
    {
        return await Context.Set<T>().ToListAsync(cancellationToken);
    }
}
