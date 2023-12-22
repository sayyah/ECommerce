using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface ISizeRepository : IRepositoryBase<Size>
{
    Task<PagedList<Size>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    Task<Size?> GetByName(string name, CancellationToken cancellationToken);
}
