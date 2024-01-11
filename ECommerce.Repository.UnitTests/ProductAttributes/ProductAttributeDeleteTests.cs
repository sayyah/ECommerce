using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using Xunit;

namespace ECommerce.Repository.UnitTests.ProductAttributes
{
    public class ProductAttributeDeleteTests : ProductAttributeBaseTest
    {
        private readonly IProductAttributeRepository _productAttributeRepository;
        public ProductAttributeDeleteTests()
        {
            _productAttributeRepository = new ProductAttributeRepository(DbContext);
        }

        [Fact]
        public async Task Delete_DeleteEntity_ReturnNull()
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
            await DbContext.ProductAttributes.AddAsync(expectedProductAttribute, CancellationToken);
            await DbContext.SaveChangesAsync(CancellationToken);
            DbContext.ChangeTracker.Clear();

            //Act
            _productAttributeRepository.Delete(expectedProductAttribute);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualProductAttribute = DbContext.ProductAttributes.Where(c => c.Id == id).ToList().FirstOrDefault();

            //Assert
            Assert.Null(actualProductAttribute);
        }

        [Fact]
        public async Task DeleteRange_DeleteEntity_ReturnZero()
        {
            //Arrange
            int expectedCount = 0;
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

            //Act
            _productAttributeRepository.DeleteRange(productAttributes);
            await UnitOfWork.SaveAsync(CancellationToken);
            int actualCount = DbContext.ProductAttributes.Count();

            //Assert
            Assert.Equal(expectedCount, actualCount);
        }
    }
}
