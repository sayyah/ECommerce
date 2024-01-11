using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IBrandRepository : IRepositoryBase<Brand>
{
    PagedList<Brand> Search(PaginationParameters paginationParameters);
    Task<Brand?> GetByName(string name, CancellationToken cancellationToken);
}
