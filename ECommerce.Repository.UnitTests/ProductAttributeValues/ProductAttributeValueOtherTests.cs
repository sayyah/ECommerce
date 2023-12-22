using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using Xunit;

namespace ECommerce.Repository.UnitTests.ProductAttributeValues
{
    public class ProductAttributeValueOtherTests : ProductAttributeValueBaseTest
    {
        private readonly IProductAttributeValueRepository _productAttributeValueRepository;
        public ProductAttributeValueOtherTests()
        {
            _productAttributeValueRepository = new ProductAttributeValueRepository(DbContext);
        }

        [Fact]
        public async Task GetById_GetSearchEntitiy_ReturnSearchEntity()
        {
            //Arrange
            int id = 1;
            string value = Guid.NewGuid().ToString();
            ProductAttributeValue expectedProductAttributeValue = new ProductAttributeValue
            {
                Id = id,
                Value = value
            };
            await DbContext.ProductAttributeValues.AddAsync(expectedProductAttributeValue, CancellationToken);
            await DbContext.SaveChangesAsync(CancellationToken);
            DbContext.ChangeTracker.Clear();

            //Act
            var actualProductAttributeValue = _productAttributeValueRepository.GetById(id);

            //Assert
            Assert.Equal(expectedProductAttributeValue.Id, actualProductAttributeValue.Id);
            Assert.Equal(expectedProductAttributeValue.Value, actualProductAttributeValue.Value);
        }

        [Fact]
        public async Task GetByIdAsync_GetSearchEntitiy_ReturnSearchEntity()
        {
            //Arrange
            int id = 1;
            string value = Guid.NewGuid().ToString();
            ProductAttributeValue expectedProductAttributeValue = new ProductAttributeValue
            {
                Id = id,
                Value = value        
            };
            await DbContext.ProductAttributeValues.AddAsync(expectedProductAttributeValue, CancellationToken);
            await DbContext.SaveChangesAsync(CancellationToken);
            DbContext.ChangeTracker.Clear();

            //Act
            var actualProductAttributeValue = await _productAttributeValueRepository.GetByIdAsync(CancellationToken , id);

            //Assert
            Assert.Equal(expectedProductAttributeValue.Id, actualProductAttributeValue.Id);
            Assert.Equal(expectedProductAttributeValue.Value, actualProductAttributeValue.Value);
        }

        [Fact]
        public async Task GetByIdWithInclude_GetSearchEntitiy_ReturnSearchEntity()
        {
            //Arrange
            int id = 1;
            string value = Guid.NewGuid().ToString();
            ProductAttribute attribute = AddProductAttribute(id);
            Product product = AddProduct(id);
            ProductAttributeValue expectedProductAttributeValue = new ProductAttributeValue
            {
                Id = id,
                Value = value,
                ProductAttribute = attribute,
                Product = product
            };
            await DbContext.ProductAttributeValues.AddAsync(expectedProductAttributeValue, CancellationToken);
            await DbContext.SaveChangesAsync(CancellationToken);
            DbContext.ChangeTracker.Clear();

            //Act
            var actualProductAttributeValue = _productAttributeValueRepository.GetByIdWithInclude("ProductAttribute,Product", id);

            //Assert
            Assert.Equal(expectedProductAttributeValue.Id, actualProductAttributeValue.Id);
            Assert.Equal(expectedProductAttributeValue.Value, actualProductAttributeValue.Value); 
        }

        [Fact]
        public async Task GetAll_CountAllEntities_ReturnsAllEntities()
        {
            //Arrange
            int expectedCount = 3;
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
            var actualProductAttributeValue = await _productAttributeValueRepository.GetAll(CancellationToken);

            //Assert
            Assert.Equal(expectedCount, actualProductAttributeValue.Count());
        }
    }
}
