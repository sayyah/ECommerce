using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IMessageRepository : IRepositoryBase<Message>
{
    PagedList<Message> Search(PaginationParameters paginationParameters);
}
