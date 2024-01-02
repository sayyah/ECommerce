using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IEmployeeRepository : IRepositoryBase<Employee>
{
    Task<PagedList<Employee>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);

    Task<Employee?> GetByName(string name, CancellationToken cancellationToken);
}
