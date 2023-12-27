using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.ProductAttributeGroups
{
    public class ProductAttributeGroupDeleteTests : BaseTests
    {
        private readonly IProductAttributeGroupRepository _productAttributeGroupRepository;
        public ProductAttributeGroupDeleteTests()
        {
            _productAttributeGroupRepository = new ProductAttributeGroupRepository(DbContext);
        }
        [Fact]
        public void Delete_DeleteEntity_ReturnNull()
        {
            //Arrange
            int id = 1;
            string name = Guid.NewGuid().ToString();
            ProductAttributeGroup expectedProductAttributeGroup = new ProductAttributeGroup
            {
                Id = id,
                Name = name
            };
            DbContext.ProductAttributeGroups.Add(expectedProductAttributeGroup);
            DbContext.SaveChanges();
            DbContext.ChangeTracker.Clear();

            //Act
            _productAttributeGroupRepository.Delete(expectedProductAttributeGroup);
            DbContext.SaveChanges();
            var actualProductAttributeValue = DbContext.ProductAttributeGroups.Where(c => c.Id == id).ToList().FirstOrDefault();

            //Assert
            Assert.Null(actualProductAttributeValue);
        }

        

        [Fact]
        public void DeleteRange_DeleteEntities_ReturnZeroCount()
        {
            //Arrange
            int expectedCount = 0;
            List<ProductAttributeGroup> productAttributeGroup =
            [
                 new ProductAttributeGroup
                 {
                     Id = 1,
                     Name = Guid.NewGuid().ToString()
                 },
                new ProductAttributeGroup
                {
                    Id = 2,
                    Name = Guid.NewGuid().ToString()
                },
                new ProductAttributeGroup
                {
                    Id = 3,
                    Name = Guid.NewGuid().ToString()
                }
            ];
            DbContext.ProductAttributeGroups.AddRange(productAttributeGroup);
            DbContext.SaveChanges();
            DbContext.ChangeTracker.Clear();

            //Act
            _productAttributeGroupRepository.DeleteRange(productAttributeGroup);
            DbContext.SaveChanges();

            //Assert
            int actualCount = DbContext.ProductAttributeGroups.Count();
            Assert.Equal(expectedCount, actualCount);
        } 
    }
}
