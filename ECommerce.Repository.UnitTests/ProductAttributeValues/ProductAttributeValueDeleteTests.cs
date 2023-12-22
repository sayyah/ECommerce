using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using Xunit;

namespace ECommerce.Repository.UnitTests.ProductAttributeValues
{
    public class ProductAttributeValueDeleteTests : ProductAttributeValueBaseTest
    {
        private readonly IProductAttributeValueRepository _productAttributeValueRepository;
        public ProductAttributeValueDeleteTests()
        {
            _productAttributeValueRepository = new ProductAttributeValueRepository(DbContext);
        }

        [Fact]
        public async Task Delete_DeleteEntity_ReturnNull()
        {
            //Arrange
            int id = 1;
            string value = Guid.NewGuid().ToString();
            ProductAttribute attribute = AddProductAttribute(id);
            Product product = AddProduct(id);
            ProductAttributeValue productAttributeValue = new ProductAttributeValue
            {
                Id = id,
                Value = value,
                ProductAttribute = attribute,
                Product = product
            };
            await DbContext.ProductAttributeValues.AddAsync(productAttributeValue, CancellationToken);
            await DbContext.SaveChangesAsync(CancellationToken);
            DbContext.ChangeTracker.Clear();

            //Act
            _productAttributeValueRepository.Delete(productAttributeValue);
            DbContext.SaveChanges();
            var actualProductAttributeValue = DbContext.ProductAttributeValues.Where(c => c.Id == id).ToList().FirstOrDefault();

            //Assert
            Assert.Null(actualProductAttributeValue);
        }

        [Fact]
        public async Task DeleteRange_DeleteEntities_ReturnZeroCount()
        {
            //Arrange
            int expectedCount = 0;
            List<ProductAttributeValue> productAttributeValues =
            [
                new ProductAttributeValue()
                {
                    Id = 1,
                    Value = Guid.NewGuid().ToString(),
                    ProductAttribute = AddProductAttribute(1),
                    Product = AddProduct(1)
                },
                new ProductAttributeValue()
                {
                    Id = 2,
                    Value = Guid.NewGuid().ToString(),
                    ProductAttribute = AddProductAttribute(2),
                    Product = AddProduct(2)
                },
                new ProductAttributeValue()
                {
                    Id = 3,
                    Value = Guid.NewGuid().ToString(),
                    ProductAttribute = AddProductAttribute(3),
                    Product = AddProduct(3)
                }
            ];
            await DbContext.ProductAttributeValues.AddRangeAsync(productAttributeValues, CancellationToken);
            await DbContext.SaveChangesAsync(CancellationToken);
            DbContext.ChangeTracker.Clear();

            //Act
            _productAttributeValueRepository.DeleteRange(productAttributeValues);
            DbContext.SaveChanges();
            int actualCount = DbContext.ProductAttributeValues.Count();

            //Assert
            Assert.Equal(expectedCount, actualCount);
        }
    }
}
