using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using Xunit;

namespace ECommerce.Repository.UnitTests.ProductAttributes
{
    public class ProductAttributeUpdateTests : ProductAttributeBaseTest
    {
        private readonly IProductAttributeRepository _productAttributeRepository;
        public ProductAttributeUpdateTests()
        {
            _productAttributeRepository = new ProductAttributeRepository(DbContext);
        }

        [Fact]
        public async Task Update_UpdateTitel_ReturnsChangedEntity()
        {
            //Arrange
            int id = 1;
            string title = Guid.NewGuid().ToString();
            AttributeType newAttributeType = AttributeType.Number;
            ProductAttributeGroup attributeGroup = AddProductAttributeGroup(id);
            ProductAttribute productAttribute = new ProductAttribute
            {
                Id = id,
                Title = title,
                AttributeType = newAttributeType,
                AttributeGroup = attributeGroup
            };
            await DbContext.ProductAttributes.AddAsync(productAttribute, CancellationToken);
            await DbContext.SaveChangesAsync(CancellationToken);
            DbContext.ChangeTracker.Clear();

            var expectedProductAttribute = DbContext.ProductAttributes.Where(c => c.Id == id).First();
            expectedProductAttribute.Title = Guid.NewGuid().ToString();

            //Act
            _productAttributeRepository.Update(expectedProductAttribute);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualProductAttribute = DbContext.ProductAttributes.Where(c => c.Id == id).ToList().First();

            //Assert
            Assert.Equal(expectedProductAttribute.Id, actualProductAttribute.Id);
            Assert.Equal(expectedProductAttribute.Title, actualProductAttribute.Title);
            Assert.Equal(expectedProductAttribute.AttributeType, actualProductAttribute.AttributeType);
        }

        [Fact]
        public async Task UpdateRange_UpdateEntities_ReturnsChangedEntities()
        {
            //Arrange
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
            await DbContext.ProductAttributes.AddRangeAsync(productAttributes, CancellationToken);
            await DbContext.SaveChangesAsync(CancellationToken);
            DbContext.ChangeTracker.Clear();

            var expectedProductAttributes = DbContext.ProductAttributes.ToList();
            expectedProductAttributes[0].Title = Guid.NewGuid().ToString();
            expectedProductAttributes[1].Title = Guid.NewGuid().ToString();
            expectedProductAttributes[2].Title = Guid.NewGuid().ToString();


            //Act
            _productAttributeRepository.UpdateRange(expectedProductAttributes);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualProductAttributes = DbContext.ProductAttributes.ToList();

            //Assert
            Assert.Equal(expectedProductAttributes[0].Title, actualProductAttributes[0].Title);
            Assert.Equal(expectedProductAttributes[0].AttributeType, actualProductAttributes[0].AttributeType);
            Assert.Equal(expectedProductAttributes[1].Title, actualProductAttributes[1].Title);
            Assert.Equal(expectedProductAttributes[1].AttributeType, actualProductAttributes[1].AttributeType);
            Assert.Equal(expectedProductAttributes[2].Title, actualProductAttributes[2].Title);
            Assert.Equal(expectedProductAttributes[2].AttributeType, actualProductAttributes[2].AttributeType);
        }
    }
}
