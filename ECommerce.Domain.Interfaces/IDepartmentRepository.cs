using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IDepartmentRepository : IRepositoryBase<Department>
{
    Task<PagedList<Department>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);

    Task<Department?> GetByTitle(string name, CancellationToken cancellationToken);
}
