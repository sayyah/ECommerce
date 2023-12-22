using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.ProductAttributes
{
    public class ProductAttributeAddTests : ProductAttributeBaseTest
    {
        private readonly IProductAttributeRepository _productAttributeRepository;
        public ProductAttributeAddTests()
        {
            _productAttributeRepository = new ProductAttributeRepository(DbContext);
        }

        [Fact]
        public async Task AddAsync_AddNewEntity_ReturnsSameEntity()
        {
            //Arrange
            int id = 1;
            string title = Guid.NewGuid().ToString();
            AttributeType attributeType = AttributeType.Number;
            ProductAttributeGroup attributeGroup = AddProductAttributeGroup(id);
            ProductAttribute expectedProductAttribute = new ProductAttribute
            {
                Id = id,
                Title = title,
                AttributeType = attributeType,
                AttributeGroup = attributeGroup
            };

            //Act
            await _productAttributeRepository.AddAsync(expectedProductAttribute, CancellationToken);
            await DbContext.SaveChangesAsync();
            ProductAttribute actualProductAttribute = DbContext.ProductAttributes.Where(x => x.Id == id).First();

            //Assert
            Assert.Equal(expectedProductAttribute.Id, actualProductAttribute.Id);
            Assert.Equal(expectedProductAttribute.Title, actualProductAttribute.Title);
            Assert.Equal(expectedProductAttribute.AttributeType, actualProductAttribute.AttributeType);
        }

        [Fact]
        public void Add_AddNewEntity_ReturnsSameEntity()
        {
            //Arrange
            int id = 1;
            string title = Guid.NewGuid().ToString();
            AttributeType attributeType = AttributeType.Number;
            ProductAttributeGroup attributeGroup = AddProductAttributeGroup(id);
            ProductAttribute expectedProductAttribute = new ProductAttribute
            {
                Id = id,
                Title = title,
                AttributeType = attributeType,
                AttributeGroup = attributeGroup
            };

            //Act
            _productAttributeRepository.Add(expectedProductAttribute);
            DbContext.SaveChanges();
            var actualProductAttribute = DbContext.ProductAttributes.Where(c => c.Id == id).ToList().First();

            //Assert
            Assert.Equal(expectedProductAttribute.Id, actualProductAttribute.Id);
            Assert.Equal(expectedProductAttribute.Title, actualProductAttribute.Title);
            Assert.Equal(expectedProductAttribute.AttributeType, actualProductAttribute.AttributeType);
        }

        [Fact]
        public void AddRange_AddListOfEntity_ReturnsAllEntity()
        {
            //Arrange
            int expectedCount = 3;
            List<ProductAttribute> productAttributes =
            [
                new ProductAttribute()
                {
                    Id = 1,
                    Title = Guid.NewGuid().ToString(),
                    AttributeType = AttributeType.Number,
                    AttributeGroup = AddProductAttributeGroup(1)
                },
                new ProductAttribute()
                {
                    Id = 2,
                    Title = Guid.NewGuid().ToString(),
                    AttributeType = AttributeType.Number,
                    AttributeGroup = AddProductAttributeGroup(2)
                },
                new ProductAttribute()
                {
                    Id = 3,
                    Title = Guid.NewGuid().ToString(),
                    AttributeType = AttributeType.Number,
                    AttributeGroup = AddProductAttributeGroup(3)
                }
            ];

            //Act
            _productAttributeRepository.AddRange(productAttributes);
            DbContext.SaveChanges();
            int actualProductAttributeCount = DbContext.ProductAttributes.Count();

            //Assert
            Assert.Equal(expectedCount, actualProductAttributeCount);
        }
       
        [Fact]
        public void Add_AddNewEntity_ThrowsException()
        {
            //Arrange
            int id = 1;
            string title = Guid.NewGuid().ToString();
            AttributeType attributeType = AttributeType.Number;
            ProductAttributeGroup attributeGroup = AddProductAttributeGroup(id);
            ProductAttribute productAttribute = new ProductAttribute
            {
                Id = id,
                Title = title,
                AttributeType = attributeType
            };

            //Act

            // Assert
            Assert.Throws<DbUpdateException>(() => _productAttributeRepository.Add(productAttribute));
        }

        [Fact]
        public async Task AddAsync_AddNewEntity_ThrowsExceptionAsync()
        {
            //Arrange
            int id = 1;
            string title = Guid.NewGuid().ToString();
            AttributeType attributeType = AttributeType.Number;
            ProductAttributeGroup attributeGroup = AddProductAttributeGroup(id);
            ProductAttribute productAttribute = new ProductAttribute
            {
                Id = id,
                Title = title,
                AttributeType = attributeType
            };

            //Act
            async Task Action()
            {
                await _productAttributeRepository.AddAsync(productAttribute, CancellationToken);
            }

            // Assert
            await Assert.ThrowsAsync<DbUpdateException>(Action);
        }
    }
}
