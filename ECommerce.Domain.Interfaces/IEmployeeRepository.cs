using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IEmployeeRepository : IAsyncRepository<Employee>
{
    Task<PagedList<Employee>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);

    Task<Employee> GetByName(string name, CancellationToken cancellationToken);
}
