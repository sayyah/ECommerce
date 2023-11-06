using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IStateRepository : IAsyncRepository<State>
{
    Task<State> GetByName(string name, CancellationToken cancellationToken);
    Task<PagedList<State>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
}
