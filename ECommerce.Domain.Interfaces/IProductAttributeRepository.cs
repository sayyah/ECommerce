using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IProductAttributeRepository : IRepositoryBase<ProductAttribute>
{
    Task<PagedList<ProductAttribute>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);

    Task<ProductAttribute?> GetByTitle(string title, CancellationToken cancellationToken);
    Task<IEnumerable<ProductAttribute>> GetAll(int productId, CancellationToken cancellationToken);

    Task<IEnumerable<ProductAttribute>> GetAllAttributeWithGroupId(int groupId, int productId,
        CancellationToken cancellationToken);
}
