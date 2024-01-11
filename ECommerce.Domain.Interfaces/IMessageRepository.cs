using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IMessageRepository : IRepositoryBase<Message>
{
    Task<PagedList<Message>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
}
