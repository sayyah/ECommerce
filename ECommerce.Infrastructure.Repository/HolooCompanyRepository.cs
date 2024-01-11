namespace ECommerce.Infrastructure.Repository;

public class HolooCompanyRepository(SunflowerECommerceDbContext context) : RepositoryBase<HolooCompany>(context),
    IHolooCompanyRepository
{
    private readonly SunflowerECommerceDbContext context = context;
}
