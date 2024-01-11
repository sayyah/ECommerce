using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IProductAttributeValueRepository : IRepositoryBase<ProductAttributeValue>
{
   PagedList<ProductAttributeValue> Search(PaginationParameters paginationParameters);
}
