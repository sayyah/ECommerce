using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IBrandRepository : IAsyncRepository<Brand>
{
    Task<PagedList<Brand>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    Task<Brand> GetByName(string name, CancellationToken cancellationToken);
}
