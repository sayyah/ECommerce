using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IColorRepository : IRepositoryBase<Color>
{
   PagedList<Color> Search(PaginationParameters paginationParameters);
    Task<Color?> GetByName(string name, CancellationToken cancellationToken);
}
