using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface ICurrencyRepository : IRepositoryBase<Currency>
{
    PagedList<Currency> Search(PaginationParameters paginationParameters);

    Task<Currency?> GetByName(string name, CancellationToken cancellationToken);
}
