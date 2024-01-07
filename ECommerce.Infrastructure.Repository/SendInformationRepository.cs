namespace ECommerce.Infrastructure.Repository;

public class SendInformationRepository(SunflowerECommerceDbContext context) : AsyncRepository<SendInformation>(context),
    ISendInformationRepository
{
    private readonly SunflowerECommerceDbContext _context = context;
}
