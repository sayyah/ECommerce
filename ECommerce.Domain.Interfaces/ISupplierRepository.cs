using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface ISupplierRepository : IRepositoryBase<Supplier>
{
    Task<PagedList<Supplier>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);

    Task<Supplier?> GetByName(string name, CancellationToken cancellationToken);
}
