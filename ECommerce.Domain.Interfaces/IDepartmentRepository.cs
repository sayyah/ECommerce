using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IDepartmentRepository : IRepositoryBase<Department>
{
    PagedList<Department> Search(PaginationParameters paginationParameters);

    Task<Department?> GetByTitle(string name, CancellationToken cancellationToken);
}
