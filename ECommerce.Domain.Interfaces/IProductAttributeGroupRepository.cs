using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IProductAttributeGroupRepository : IAsyncRepository<ProductAttributeGroup>
{
    Task<PagedList<ProductAttributeGroup>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);

    Task<ProductAttributeGroup> GetByName(string name, CancellationToken cancellationToken);

    Task<List<ProductAttributeGroup>> GetWithInclude(CancellationToken cancellationToken);

    Task<IEnumerable<ProductAttributeGroup>> GetAllAttributeWithProductId(int productId,
        CancellationToken cancellationToken);

    Task<List<ProductAttributeGroup>> AddWithAttributeValue(List<ProductAttributeGroup> productAttributeGroups,
        int productId,
        CancellationToken cancellationToken);
}
