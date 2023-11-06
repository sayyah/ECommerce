using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IColorRepository : IAsyncRepository<Color>
{
    Task<PagedList<Color>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    Task<Color> GetByName(string name, CancellationToken cancellationToken);
}
