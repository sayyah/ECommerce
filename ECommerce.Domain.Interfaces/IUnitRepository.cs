using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IUnitRepository : IRepositoryBase<Unit>
{
    PagedList<Unit> Search(PaginationParameters paginationParameters);
    Task<Unit?> GetByName(string name, CancellationToken cancellationToken);

    void AddAll(IEnumerable<Unit> units);

    int? GetId(int? unitCode, CancellationToken cancellationToken);
}
