using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class SendInformationRepository : RepositoryBase<SendInformation>, ISendInformationRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public SendInformationRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }
}
