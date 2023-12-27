using ECommerce.Domain.Entities;
using ECommerce.Repository.UnitTests.Base;

namespace ECommerce.Repository.UnitTests.ProductAttributeValues;

public class ProductAttributeValueBaseTest : BaseTests
{
    public ProductAttribute AddProductAttribute(int Id)
    {
        int id = Id;
        ProductAttribute productAttribute = new ProductAttribute
        {
            Id = id,
            Title = "چینی زرین",
            AttributeType = AttributeType.Text
        };
        return productAttribute;
    }

    public Product AddProduct(int Id)
    {
        int id = Id;
        Product product = new Product
        {
            Id = id,
            Name = "لیوان",
            Url = Guid.NewGuid().ToString(),
            Description = "لیوان شیشه ای ایرانی شفاف",
            MinOrder = 1,
            MaxOrder = 10
        };
        return product;
    }
}
