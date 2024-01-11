using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.ProductAttributeValues
{
    public class ProductAttributeValueAddTests : ProductAttributeValueBaseTest
    {
        private readonly IProductAttributeValueRepository _productAttributeValueRepository;
        public ProductAttributeValueAddTests()
        {
            _productAttributeValueRepository = new ProductAttributeValueRepository(DbContext);
        }

        [Fact]
        public async Task AddAsync_AddNewEntity_ReturnsSameEntity()
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

            //Act
            await _productAttributeValueRepository.AddAsync(expectedProductAttributeValue, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
            ProductAttributeValue actualProductAttributeValue = DbContext.ProductAttributeValues.Where(x => x.Id == id).First();

            //Assert
            Assert.Equal(expectedProductAttributeValue.Id, actualProductAttributeValue.Id);
            Assert.Equal(expectedProductAttributeValue.Value, actualProductAttributeValue.Value);
        }

        [Fact]
        public async Task Add_AddNewEntity_ReturnsSameEntity()
        {
            //Arrange
            int id = 1;
            string value = Guid.NewGuid().ToString();
            ProductAttribute attribute = AddProductAttribute(id);
            Product product = AddProduct(id);
            ProductAttributeValue expectedProductAttributeValue = new()
            {
                Id = id,
                Value = value,
                ProductAttribute = attribute,
                Product = product
            };

            //Act
            _productAttributeValueRepository.Add(expectedProductAttributeValue);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualProductAttributeValue = DbContext.ProductAttributeValues.Where(c => c.Id == id).First();

            //Assert
            Assert.Equal(expectedProductAttributeValue.Id, actualProductAttributeValue.Id);
            Assert.Equal(expectedProductAttributeValue.Value, actualProductAttributeValue.Value);
        }

        [Fact]
        public async Task AddRange_AddIistOfEntity_SameCountEntities()
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

            //Act
            _productAttributeValueRepository.AddRange(productAttributeValues);
            await UnitOfWork.SaveAsync(CancellationToken);
            int actualProductAttributeValueCount = DbContext.ProductAttributeValues.Count();

            //Assert
            Assert.Equal(expectedCount, actualProductAttributeValueCount);
        }

        //[Fact]
        //public void Add_AddNewEntity_ThrowsException()
        //{
        //    //Arrange
        //    int id = 1;
        //    string value = Guid.NewGuid().ToString();
        //    ProductAttribute attribute = AddProductAttribute(id);
        //    Product product = AddProduct(id);
        //    ProductAttributeValue productAttributeValue = new()
        //    {
        //        Id = id,
        //        Value = value,
        //    };

        //    //Act

        //    // Assert
        //    Assert.Throws<DbUpdateException>(() => _productAttributeValueRepository.Add(productAttributeValue));
        //}

        //[Fact]
        //public async Task AddAsync_AddNewEntity_ThrowsExceptionAsync()
        //{
        //    //Arrange
        //    int id = 1;
        //    string value = Guid.NewGuid().ToString();
        //    ProductAttribute attribute = AddProductAttribute(id);
        //    Product product = AddProduct(id);
        //    ProductAttributeValue productAttributeValue = new()
        //    {
        //        Id = id,
        //        Value = value,
        //    };

        //    //Act
        //    async Task Action()
        //    {
        //        await _productAttributeValueRepository.AddAsync(productAttributeValue, CancellationToken);
        //    }

        //    // Assert
        //    await Assert.ThrowsAsync<DbUpdateException>(Action);
        //}
    }
}
