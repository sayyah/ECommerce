using ECommerce.Domain.Entities;
using ECommerce.Repository.UnitTests.Base;

namespace ECommerce.Repository.UnitTests.ProductAttributes
{
    public class ProductAttributeBaseTest : BaseTests
    {
        public ProductAttributeGroup AddProductAttributeGroup(int Id)
        {
            ProductAttributeGroup productAttributeGroup = new ProductAttributeGroup
            {
                Id = Id,
                Name = Guid.NewGuid().ToString(),
                CreatorUserId = Id,
                EditorUserId = Id,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            return productAttributeGroup;
        }

    }
}

