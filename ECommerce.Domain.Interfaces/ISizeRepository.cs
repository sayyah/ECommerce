using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface ISizeRepository : IRepositoryBase<Size>
{
    PagedList<Size> Search(PaginationParameters paginationParameters);
    Task<Size?> GetByName(string name, CancellationToken cancellationToken);
}
