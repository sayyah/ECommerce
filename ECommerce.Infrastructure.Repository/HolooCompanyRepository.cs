using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class HolooCompanyRepository : RepositoryBase<HolooCompany>, IHolooCompanyRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public HolooCompanyRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }
}
