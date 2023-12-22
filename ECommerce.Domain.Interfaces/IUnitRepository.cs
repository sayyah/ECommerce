using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IUnitRepository : IRepositoryBase<Unit>
{
    Task<PagedList<Unit>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    Task<Unit?> GetByName(string name, CancellationToken cancellationToken);

    void AddAll(IEnumerable<Unit> units);

    int? GetId(int? unitCode, CancellationToken cancellationToken);
}
