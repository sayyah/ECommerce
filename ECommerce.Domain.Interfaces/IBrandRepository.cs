using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IContactRepository : IRepositoryBase<Contact>
{
    PagedList<Contact> Search(PaginationParameters paginationParameters);
    Task<Contact?> GetByName(string name, CancellationToken cancellationToken);
    Task<Contact?> GetByEmail(string email, CancellationToken cancellationToken);
    Task<Contact?> GetRepetitive(Contact contact, CancellationToken cancellationToken);
}
