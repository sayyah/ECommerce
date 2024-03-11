using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class ShippingRepository(SunflowerECommerceDbContext context) : RepositoryBase<Shipping>(context),
    IShippingRepository
{
    private readonly SunflowerECommerceDbContext context = context;
}
