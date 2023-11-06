using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IUnitRepository : IAsyncRepository<Unit>
{
    Task<PagedList<Unit>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    Task<Unit> GetByName(string name, CancellationToken cancellationToken);

    Task<int> AddAll(IEnumerable<Unit> units, CancellationToken cancellationToken);

    int? GetId(int? unitCode, CancellationToken cancellationToken);
}
