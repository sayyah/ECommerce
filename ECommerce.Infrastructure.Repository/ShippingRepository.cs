namespace ECommerce.Infrastructure.Repository;

public class ShippingRepository(SunflowerECommerceDbContext context) : AsyncRepository<Shipping>(context),
    IShippingRepository
{
    private readonly SunflowerECommerceDbContext _context = context;
}
