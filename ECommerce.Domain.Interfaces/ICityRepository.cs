using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface ICityRepository : IRepositoryBase<City>
{
    Task<City?> GetByName(string name, CancellationToken cancellationToken);

    PagedList<City> Search(PaginationParameters paginationParameters);
}
