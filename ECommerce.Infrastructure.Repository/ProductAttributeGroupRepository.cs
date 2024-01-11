namespace ECommerce.Infrastructure.Repository;

public class ProductAttributeGroupRepository
    (SunflowerECommerceDbContext context) : RepositoryBase<ProductAttributeGroup>(context),
        IProductAttributeGroupRepository
{
    public async Task<ProductAttributeGroup?> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.ProductAttributeGroups.Where(x => x.Name == name)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<ProductAttributeGroup>> GetWithInclude(CancellationToken cancellationToken)
    {
        return await context.ProductAttributeGroups.Include(nameof(ProductAttributeGroup.Attribute))
            .ToListAsync(cancellationToken);
        ;
    }

    public async Task<List<ProductAttributeGroup>?> GetAllAttributeWithProductId(int productId,
        CancellationToken cancellationToken)
    {
        var group = await context.ProductAttributeGroups
            //.Where(p => p.Products.Any(x => x.Id == productId))
            .Include(a => a.Attribute)
            .ToListAsync(cancellationToken);
        var productValues = await context.ProductAttributeValues.Where(x => x.ProductId == productId)
            .ToListAsync(cancellationToken);

        foreach (var productAttributeGroup in group)
            foreach (var attribute in productAttributeGroup.Attribute)
            {
                var value = productValues.FirstOrDefault(x => x.ProductAttributeId == attribute.Id);
                if (value == null) attribute.AttributeValue.Add(new ProductAttributeValue());
                // else
                // {
                //     attribute.AttributeValue.Add(value);
                // }
            }

        return group;
    }

    public List<ProductAttributeGroup> AddWithAttributeValue(List<ProductAttributeGroup> productAttributeGroups, int productId)
    {
        foreach (var productAttributeGroup in productAttributeGroups)
            foreach (var productAttribute in productAttributeGroup.Attribute)
                if (productAttribute.AttributeValue[0].Id > 0)
                {
                    var entity =
                        context.ProductAttributeValues.First(x => x.Id == productAttribute.AttributeValue[0].Id);
                    entity.Value = productAttribute.AttributeValue[0].Value.Trim();
                    context.ProductAttributeValues.Update(entity);
                }
                else
                {
                    if (productAttribute.AttributeValue[0].Value != null)
                        context.ProductAttributeValues.Add(new ProductAttributeValue
                        {
                            ProductId = productId,
                            Value = productAttribute.AttributeValue[0].Value.Trim(),
                            ProductAttributeId = productAttribute.Id
                        });
                }

        return productAttributeGroups;
    }

    public async Task<PagedList<ProductAttributeGroup>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<ProductAttributeGroup>.ToPagedList(
            await context.ProductAttributeGroups.Where(x => x.Name.Contains(paginationParameters.Search))
                .AsNoTracking().OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
