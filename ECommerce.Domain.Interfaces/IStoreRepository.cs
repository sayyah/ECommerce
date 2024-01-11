using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IStoreRepository : IRepositoryBase<Store>
{
    PagedList<Store> Search(PaginationParameters paginationParameters);
    Task<Store?> GetByName(string name, CancellationToken cancellationToken);
}
