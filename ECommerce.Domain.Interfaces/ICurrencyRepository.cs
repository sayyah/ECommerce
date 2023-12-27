using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface ICurrencyRepository : IRepositoryBase<Currency>
{
    Task<PagedList<Currency>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);

    Task<Currency?> GetByName(string name, CancellationToken cancellationToken);
}
