using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface ISupplierRepository : IRepositoryBase<Supplier>
{
   PagedList<Supplier> Search(PaginationParameters paginationParameters);

    Task<Supplier?> GetByName(string name, CancellationToken cancellationToken);
}
