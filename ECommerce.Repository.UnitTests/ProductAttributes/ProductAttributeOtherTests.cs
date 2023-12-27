using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using Xunit;

namespace ECommerce.Repository.UnitTests.ProductAttributes
{
    public class ProductAttributeOtherTests : ProductAttributeBaseTest
    {
        private readonly IProductAttributeRepository _productAttributeRepository;
        public ProductAttributeOtherTests()
        {
            _productAttributeRepository = new ProductAttributeRepository(DbContext);
        }

        [Fact]
        public async Task GetById_GetLastEntitiy_ReturnLastEntity()
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
            var actualProductAttribute = await _productAttributeRepository.GetByIdAsync( CancellationToken , id);

            //Assert
            Assert.Equal(expectedProductAttribute.Id, actualProductAttribute.Id);
            Assert.Equal(expectedProductAttribute.Title, actualProductAttribute.Title);
        }

        [Fact]
        public async Task GetByIdAsync_GetLastEntitiy_ReturnLastEntity()
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
            var actualProductAttribute = await _productAttributeRepository.GetByIdAsync(CancellationToken, id);

            //Assert
            Assert.Equal(expectedProductAttribute.Id, actualProductAttribute.Id);
            Assert.Equal(expectedProductAttribute.Title, actualProductAttribute.Title);
        }

        //[Fact]
        //public async Task GetByIdWithInclude_GetSearchEntitiy_ReturnSearchEntity()
        //{
        //    //Arrange
        //    int id = 1;
        //    string title = Guid.NewGuid().ToString();
        //    AttributeType attributeType = AttributeType.Number;
        //    ProductAttributeGroup attributeGroup = AddProductAttributeGroup(id);
        //    ProductAttribute expectedProductAttribute = new ProductAttribute
        //    {
        //        Id = id,
        //        Title = title,
        //        AttributeType = attributeType,
        //        AttributeGroup = attributeGroup
        //    };
        //    await DbContext.ProductAttributes.AddAsync(expectedProductAttribute, CancellationToken);
        //    await DbContext.SaveChangesAsync(CancellationToken);
        //    DbContext.ChangeTracker.Clear();

        //    //Act
        //    var actualProductAttributeValue = _productAttributeRepository.GetByIdWithInclude("ProductAttributeGroup", id);

        //    //Assert
        //    Assert.Equal(expectedProductAttribute.Id, actualProductAttributeValue.Id);
        //    Assert.Equal(expectedProductAttribute.Title, actualProductAttributeValue.Title);
        //}

        [Fact]
        public async Task GetByTitel_GetLastEntitiy_ReturnEntity()
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
            var actualProductAttribute =  _productAttributeRepository.GetByTitle(title, CancellationToken);

            //Assert
            Assert.Equal(id, actualProductAttribute.Id);
        }
    }
}
