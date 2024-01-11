using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.ProductAttributeGroups
{
    public class ProductAttributeGrooupAddTests :BaseTests
    {
        private readonly IProductAttributeGroupRepository _productAttributeGroupRepository;
        public ProductAttributeGrooupAddTests()
        {
            _productAttributeGroupRepository = new ProductAttributeGroupRepository(DbContext);
        }

        [Fact]
        public async Task Add_AddNewEntity_ReturnsSameEntity()
        {
            //Arrange
            int id = 1;
            string name = Guid.NewGuid().ToString();
            ProductAttributeGroup expectedProductAttributeGroup = new ProductAttributeGroup
            {
                Id = id,
                Name = name
            };

            //Act
            _productAttributeGroupRepository.Add(expectedProductAttributeGroup);
            await UnitOfWork.SaveAsync(CancellationToken);
            ProductAttributeGroup actualProductAttributeGroup = DbContext.ProductAttributeGroups.Where(x => x.Id == id).First();

            //Assert
            Assert.Equal(expectedProductAttributeGroup.Id, actualProductAttributeGroup.Id);
            Assert.Equal(expectedProductAttributeGroup.Name, actualProductAttributeGroup.Name);
        }

        [Fact]
        public async Task AddRange_AddListOfEntity_SameCountEntities()
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
                },
            ];

            //Act
            _productAttributeGroupRepository.AddRange(productAttributeGroup);
            await UnitOfWork.SaveAsync(CancellationToken);
            int actualProductAttributeGroupCount = DbContext.ProductAttributeGroups.Count();

            //Assert
            Assert.Equal(expectedCount, actualProductAttributeGroupCount);
        }
    }
}
