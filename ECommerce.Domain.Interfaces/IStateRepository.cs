using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IStateRepository : IRepositoryBase<State>
{
    Task<State?> GetByName(string name, CancellationToken cancellationToken);
    PagedList<State> Search(PaginationParameters paginationParameters);
}
