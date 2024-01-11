namespace ECommerce.Infrastructure.Repository;

public class SendInformationRepository(SunflowerECommerceDbContext context) : RepositoryBase<SendInformation>(context),
    ISendInformationRepository
{
    private readonly SunflowerECommerceDbContext context = context;
}
