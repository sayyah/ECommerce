namespace ECommerce.Infrastructure.Repository;

public class HolooCompanyRepository : AsyncRepository<HolooCompany>, IHolooCompanyRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public HolooCompanyRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }
}
