using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.ProductAttributeGroups
{
    public class ProductAttributeGroupOthereTests : BaseTests
    {
        private readonly IProductAttributeGroupRepository _productAttributeGroupRepository;
        public ProductAttributeGroupOthereTests()
        {
            _productAttributeGroupRepository = new ProductAttributeGroupRepository(DbContext);
        }

        [Fact]
        public void GetById_GetSearchEntitiy_ReturnSearchEntity()
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
            var actualProductAttributeGroup = _productAttributeGroupRepository.GetById(id);

            //Assert
            Assert.Equal(expectedProductAttributeGroup.Id, actualProductAttributeGroup.Id);
            Assert.Equal(expectedProductAttributeGroup.Name, actualProductAttributeGroup.Name);
        }

        [Fact]
        public async Task GetByIdAsync_GetSearchEntitiy_ReturnSearchEntity()
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
            var actualProductAttributeGroup = await _productAttributeGroupRepository.GetByIdAsync(CancellationToken, id);

            //Assert
            Assert.Equal(expectedProductAttributeGroup.Id, actualProductAttributeGroup.Id);
            Assert.Equal(expectedProductAttributeGroup.Name, actualProductAttributeGroup.Name);
        }

        [Fact]
        public async Task GetAll_CountAllEntities_ReturnsAllEntities()
        {
            //Arrange
            int expectedCount = 3;
            List<ProductAttributeGroup> productAttributeGroup =
            [
                new ProductAttributeGroup()
                {
                    Id = 1,
                    Name = Guid.NewGuid().ToString()
                },
                new ProductAttributeGroup()
                {
                    Id = 2,
                    Name = Guid.NewGuid().ToString()
                },
                new ProductAttributeGroup()
                {
                    Id = 3,
                    Name = Guid.NewGuid().ToString()
                }
            ];
            DbContext.ProductAttributeGroups.AddRange(productAttributeGroup);
            DbContext.SaveChanges();
            DbContext.ChangeTracker.Clear();

            //Act
            var actualProductAttributeGroup = await _productAttributeGroupRepository.GetAll(CancellationToken);

            //Assert
            Assert.Equal(expectedCount, actualProductAttributeGroup.Count());
        }
    }
}
