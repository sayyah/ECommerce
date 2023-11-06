using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IContactRepository : IAsyncRepository<Contact>
{
    Task<PagedList<Contact>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    Task<Contact?> GetByName(string name, CancellationToken cancellationToken);
    Task<Contact?> GetByEmail(string email, CancellationToken cancellationToken);
    Task<Contact?> GetRepetitive(Contact contact, CancellationToken cancellationToken);
}
