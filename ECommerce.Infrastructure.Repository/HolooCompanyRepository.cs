namespace ECommerce.Infrastructure.Repository;

public class HolooCompanyRepository(SunflowerECommerceDbContext context) : AsyncRepository<HolooCompany>(context),
    IHolooCompanyRepository
{
    private readonly SunflowerECommerceDbContext _context = context;
}
