using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IProductAttributeGroupRepository : IRepositoryBase<ProductAttributeGroup>
{
    PagedList<ProductAttributeGroup> Search(PaginationParameters paginationParameters);

    Task<ProductAttributeGroup?> GetByName(string name, CancellationToken cancellationToken);

    Task<List<ProductAttributeGroup>> GetWithInclude(CancellationToken cancellationToken);

    Task<List<ProductAttributeGroup>?> GetAllAttributeWithProductId(int productId,
        CancellationToken cancellationToken);

    List<ProductAttributeGroup> AddWithAttributeValue(List<ProductAttributeGroup> productAttributeGroups, int productId);
}
