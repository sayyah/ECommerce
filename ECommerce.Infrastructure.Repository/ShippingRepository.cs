using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class ShippingRepository : RepositoryBase<Shipping>, IShippingRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public ShippingRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }
}
