namespace ECommerce.Infrastructure.Repository;

public class MessageRepository(SunflowerECommerceDbContext context) : RepositoryBase<Message>(context),
    IMessageRepository
{
    public async Task<PagedList<Message>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Message>.ToPagedList(
            await context.Messages.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
