using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IProductAttributeRepository : IRepositoryBase<ProductAttribute>
{
    PagedList<ProductAttribute> Search(PaginationParameters paginationParameters);

    Task<ProductAttribute?> GetByTitle(string title, CancellationToken cancellationToken);
    Task<IEnumerable<ProductAttribute>> GetAll(int productId, CancellationToken cancellationToken);

    Task<IEnumerable<ProductAttribute>> GetAllAttributeWithGroupId(int groupId, int productId,
        CancellationToken cancellationToken);
}
