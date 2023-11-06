using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IStoreRepository : IAsyncRepository<Store>
{
    Task<PagedList<Store>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    Task<Store> GetByName(string name, CancellationToken cancellationToken);
}
