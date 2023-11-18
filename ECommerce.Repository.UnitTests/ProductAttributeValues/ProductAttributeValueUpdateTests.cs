using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using Xunit;

namespace ECommerce.Repository.UnitTests.ProductAttributeValues
{
    public class ProductAttributeValueUpdateTests : ProductAttributeValueBaseTest
    {
        private readonly IProductAttributeValueRepository _productAttributeValueRepository;
        public ProductAttributeValueUpdateTests()
        {
            _productAttributeValueRepository = new ProductAttributeValueRepository(DbContext);
        }

        [Fact]
        public async Task Update_UpdateValue_ReturnsChangedEntity()
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
            var expectedProductAttributeValue = DbContext.ProductAttributeValues.Where(c => c.Id == id).First();
            expectedProductAttributeValue.Value = Guid.NewGuid().ToString();

            //Act
            _productAttributeValueRepository.Update(expectedProductAttributeValue);
            await UnitOfWork.SaveAsync(CancellationToken);
            ProductAttributeValue actualProductAttributeValue = DbContext.ProductAttributeValues.Where(c => c.Id == id).First();

            //Assert
            Assert.Equal(expectedProductAttributeValue.Id, actualProductAttributeValue.Id);
            Assert.Equal(expectedProductAttributeValue.Value, actualProductAttributeValue.Value);
        }

        [Fact]
        public async Task UpdateRange_UpdateValue_ReturnsChangedEntities()
        {
            //Arrange
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
            var expectedProductAttributeValue = DbContext.ProductAttributeValues.ToList();
            expectedProductAttributeValue[0].Value = Guid.NewGuid().ToString();
            expectedProductAttributeValue[1].Value = Guid.NewGuid().ToString();
            expectedProductAttributeValue[2].Value = Guid.NewGuid().ToString();

            //Act
            _productAttributeValueRepository.UpdateRange(expectedProductAttributeValue);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualProductAttributeValue = DbContext.ProductAttributeValues.ToList();

            //Assert
            Assert.Equal(expectedProductAttributeValue[0].Value, actualProductAttributeValue[0].Value);
            Assert.Equal(expectedProductAttributeValue[1].Value, actualProductAttributeValue[1].Value);
            Assert.Equal(expectedProductAttributeValue[2].Value, actualProductAttributeValue[2].Value);
        }
    }
}
