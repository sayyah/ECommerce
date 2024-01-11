using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IEmployeeRepository : IRepositoryBase<Employee>
{
    PagedList<Employee> Search(PaginationParameters paginationParameters);

    Task<Employee?> GetByName(string name, CancellationToken cancellationToken);
}
