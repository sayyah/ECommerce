using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.ProductAttributeGroups
{
    public class ProductAttributeGroupUpdateTests : BaseTests
    {
        private readonly IProductAttributeGroupRepository _productAttributeGroupRepository;
        public ProductAttributeGroupUpdateTests()
        {
            _productAttributeGroupRepository = new ProductAttributeGroupRepository(DbContext);
        }

        [Fact]
        public async Task Update_UpdateName_ReturnsChangedEntity()
        {
            //Arrange
            int id = 1;
            string name = Guid.NewGuid().ToString();
            ProductAttributeGroup productAttributeGroup = new ProductAttributeGroup
            {
                Id = id,
                Name = name
            };
            DbContext.ProductAttributeGroups.Add(productAttributeGroup);
            DbContext.SaveChanges();
            DbContext.ChangeTracker.Clear();
            var expectedProductAttributeGroup = DbContext.ProductAttributeGroups.Where(c => c.Id == id).First();
            expectedProductAttributeGroup.Name = Guid.NewGuid().ToString();

            //Act
            _productAttributeGroupRepository.Update(expectedProductAttributeGroup);
            await UnitOfWork.SaveAsync(CancellationToken);
            ProductAttributeGroup actualProductAttributeGroup = DbContext.ProductAttributeGroups.Where(c => c.Id == id).First();

            //Assert
            Assert.Equal(expectedProductAttributeGroup.Id, actualProductAttributeGroup.Id);
            Assert.Equal(expectedProductAttributeGroup.Name, actualProductAttributeGroup.Name);
        }

        [Fact]
        public async Task UpdateRange_UpdateName_ReturnsChangedEntities()
        {
            //Arrange
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
                },
            ];
            DbContext.ProductAttributeGroups.AddRange(productAttributeGroup);
            DbContext.SaveChanges();
            DbContext.ChangeTracker.Clear();
            var expectedProductAttributeGroup = DbContext.ProductAttributeGroups.ToList();
            expectedProductAttributeGroup[0].Name = Guid.NewGuid().ToString();
            expectedProductAttributeGroup[1].Name = Guid.NewGuid().ToString();
            expectedProductAttributeGroup[2].Name = Guid.NewGuid().ToString();

            //Act
            _productAttributeGroupRepository.UpdateRange(expectedProductAttributeGroup);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualProductAttributeGroup = DbContext.ProductAttributeGroups.ToList();

            //Assert
            Assert.Equal(expectedProductAttributeGroup[0].Name, actualProductAttributeGroup[0].Name);
            Assert.Equal(expectedProductAttributeGroup[1].Name, actualProductAttributeGroup[1].Name);
            Assert.Equal(expectedProductAttributeGroup[2].Name, actualProductAttributeGroup[2].Name);
        }
    }
}
