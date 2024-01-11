using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class MessageRepository(SunflowerECommerceDbContext context) : RepositoryBase<Message>(context),
    IMessageRepository
{
    public PagedList<Message> Search(PaginationParameters paginationParameters)
    {
        return PagedList<Message>.ToPagedList(
            context.Messages.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
