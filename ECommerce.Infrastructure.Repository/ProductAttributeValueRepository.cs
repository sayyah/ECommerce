using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class ProductAttributeValueRepository : RepositoryBase<ProductAttributeValue>, IProductAttributeValueRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public ProductAttributeValueRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PagedList<ProductAttributeValue>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<ProductAttributeValue>.ToPagedList(
            await _context.ProductAttributeValues.Where(x => x.Value.Contains(paginationParameters.Search))
                .AsNoTracking().OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }

    public async Task<IEnumerable<ProductAttributeValue>> GetAll(int productAttributeId)
    {
        return await _context.ProductAttributeValues.Where(x => x.ProductAttributeId == productAttributeId)
            .ToListAsync();
    }
}
