using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class ProductAttributeValueRepository
    (SunflowerECommerceDbContext context) : RepositoryBase<ProductAttributeValue>(context),
        IProductAttributeValueRepository
{
    public async Task<PagedList<ProductAttributeValue>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<ProductAttributeValue>.ToPagedList(
            await context.ProductAttributeValues.Where(x => x.Value.Contains(paginationParameters.Search))
                .AsNoTracking().OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }

    public async Task<IEnumerable<ProductAttributeValue>> GetAll(int productAttributeId)
    {
        return await context.ProductAttributeValues.Where(x => x.ProductAttributeId == productAttributeId)
            .ToListAsync();
    }
}
