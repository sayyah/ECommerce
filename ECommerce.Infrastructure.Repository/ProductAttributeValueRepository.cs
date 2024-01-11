using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class ProductAttributeValueRepository
    (SunflowerECommerceDbContext context) : RepositoryBase<ProductAttributeValue>(context),
        IProductAttributeValueRepository
{
    public PagedList<ProductAttributeValue> Search(PaginationParameters paginationParameters)
    {
        return PagedList<ProductAttributeValue>.ToPagedList(
            context.ProductAttributeValues.Where(x => x.Value.Contains(paginationParameters.Search))
                .AsNoTracking().OrderBy(on => on.Id),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }

    public async Task<IEnumerable<ProductAttributeValue>> GetAll(int productAttributeId)
    {
        return await context.ProductAttributeValues.Where(x => x.ProductAttributeId == productAttributeId)
            .ToListAsync();
    }
}
