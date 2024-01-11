using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface ICityRepository : IRepositoryBase<City>
{
    Task<City?> GetByName(string name, CancellationToken cancellationToken);

    Task<PagedList<City>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
}
