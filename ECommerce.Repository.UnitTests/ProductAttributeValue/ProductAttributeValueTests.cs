using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Repository.UnitTests.ProductAttributeValueTests
{
    public class ProductAttributeValueTests : BaseTests
    {
        private readonly IProductAttributeValueRepository _productAttributeValueRepository;

        public ProductAttributeValueTests()
        {
            _productAttributeValueRepository = new ProductAttributeValueRepository(DbContext);
        }

        [Fact]
        public async Task AddAsync_AddNewEntity_ReturnsSameEntity()
        {
            //Arrange
            int id = 2;
            var value = Guid.NewGuid().ToString(); 

            ProductAttributeValue productAttributeValue = new ProductAttributeValue
            {
                Id = id,
                Value = value
            };

            //Act
            ProductAttributeValue newProductAttributeValue = await _productAttributeValueRepository.AddAsync(productAttributeValue, CancellationToken);

            //Assert
            Assert.Equal(id, newProductAttributeValue.Id);
            Assert.Equal(value, newProductAttributeValue.Value);
        }
        [Fact]
        public async Task Add_AddNewEntity_ReturnsSameEntity()
        {
            //Arrange
            int id = 2;
            var value = Guid.NewGuid().ToString();

            ProductAttributeValue productAttributeValue = new ProductAttributeValue
            {
                Id = id,
                Value = value
            };

            //Act
            _productAttributeValueRepository.Add(productAttributeValue);

            //Assert
            var product_AttributeValue = DbContext.ProductAttributeValues.Where(c => c.Id == id).ToList().FirstOrDefault();
            Assert.Equal(id, product_AttributeValue.Id);
            Assert.Equal(value, product_AttributeValue.Value);
        }
        [Fact]
        public async Task UpdateAsync_UpdateEntity_ReturnsChangedEntity()
        {
            //Arrange
            int id = 2;
            var value = Guid.NewGuid().ToString();

            ProductAttributeValue productAttributeValue = new ProductAttributeValue
            {
                Id = id,
                Value = Guid.NewGuid().ToString()
            };
            await DbContext.ProductAttributeValues.AddAsync(productAttributeValue, CancellationToken);
            await DbContext.SaveChangesAsync(CancellationToken);
            DbContext.ChangeTracker.Clear();

            ProductAttributeValue editProductAttributeValue = new ProductAttributeValue
            {
                Id = id,
                Value = value
            };

            //Act
            ProductAttributeValue newProductAttributeValue = await _productAttributeValueRepository.UpdateAsync(editProductAttributeValue, CancellationToken);

            //Assert
            Assert.Equal(id, newProductAttributeValue.Id);
            Assert.Equal(value, newProductAttributeValue.Value);
        }
        [Fact]
        public async Task Update_UpdateEntity_ReturnsChangedEntity()
        {
            //Arrange
            int id = 2;
            var value = Guid.NewGuid().ToString();

            ProductAttributeValue productAttributeValue = new ProductAttributeValue
            {
                Id = id,
                Value = Guid.NewGuid().ToString()
            };
            await DbContext.ProductAttributeValues.AddAsync(productAttributeValue, CancellationToken);
            await DbContext.SaveChangesAsync(CancellationToken);
            DbContext.ChangeTracker.Clear();

            ProductAttributeValue editProductAttributeValue = new ProductAttributeValue
            {
                Id = id,
                Value = value
            };

            //Act
            _productAttributeValueRepository.Update(editProductAttributeValue);

            //Assert
            var product_AttributeValue = DbContext.ProductAttributeValues.Where(c => c.Id == id).ToList().FirstOrDefault();
            Assert.Equal(id, product_AttributeValue.Id);
            Assert.Equal(value, product_AttributeValue.Value);
        }
        [Fact]
        public async Task DeleteAsync_DeleteEntity_ReturnNull()
        {
            //Arrange
            int id = 2;
            var value = Guid.NewGuid().ToString();

            ProductAttributeValue productAttributeValue = new ProductAttributeValue
            {
                Id = id,
                Value = value
            };
            await DbContext.ProductAttributeValues.AddAsync(productAttributeValue, CancellationToken);
            await DbContext.SaveChangesAsync(CancellationToken);
            DbContext.ChangeTracker.Clear();

            //Act
            await _productAttributeValueRepository.DeleteAsync(productAttributeValue, CancellationToken);

            //Assert
            var product_AttributeValue = DbContext.ProductAttributeValues.Where(c => c.Id == id).ToList().FirstOrDefault();
            Assert.Equal(product_AttributeValue, null);
        }
        [Fact]
        public async Task Delete_DeleteEntity_ReturnNull()
        {
            //Arrange
            int id = 2;
            var value = Guid.NewGuid().ToString();

            ProductAttributeValue productAttributeValue = new ProductAttributeValue
            {
                Id = id,
                Value = value
            };
            await DbContext.ProductAttributeValues.AddAsync(productAttributeValue, CancellationToken);
            await DbContext.SaveChangesAsync(CancellationToken);
            DbContext.ChangeTracker.Clear();

            //Act
            _productAttributeValueRepository.Delete(productAttributeValue);

            //Assert
            var product_AttributeValue = DbContext.ProductAttributeValues.Where(c => c.Id == id).ToList().FirstOrDefault();
            Assert.Equal(product_AttributeValue, null);
        }
        [Fact]
        public async Task GetById_GetLastEntitiy_ReturnLastEntity()
        {
            //Arrange
            int id = 2;
            var value = Guid.NewGuid().ToString();

            ProductAttributeValue productAttributeValue = new ProductAttributeValue
            {
                Id = id,
                Value = value
            };
            await DbContext.ProductAttributeValues.AddAsync(productAttributeValue, CancellationToken);
            await DbContext.SaveChangesAsync(CancellationToken);
            DbContext.ChangeTracker.Clear();

            //Act
            var product_AttributeValue = _productAttributeValueRepository.GetById(id);

            //Assert
            Assert.Equal(id, product_AttributeValue.Id);
        }
        [Fact]
        public async Task GetAll_CountAllEntities_ReturnsTwoEntities()
        {
            //Arrange
            int id = 2;
            var value = Guid.NewGuid().ToString();

            ProductAttributeValue productAttributeValue = new ProductAttributeValue
            {
                Id = id,
                Value = value
            };
            await DbContext.ProductAttributeValues.AddAsync(productAttributeValue, CancellationToken);
            await DbContext.SaveChangesAsync(CancellationToken);
            DbContext.ChangeTracker.Clear();

            id = 3;
            value = Guid.NewGuid().ToString();

            productAttributeValue = new ProductAttributeValue
            {
                Id = id,
                Value = value
            };
            await DbContext.ProductAttributeValues.AddAsync(productAttributeValue, CancellationToken);
            await DbContext.SaveChangesAsync(CancellationToken);
            DbContext.ChangeTracker.Clear();

            //Act
            var newProductAttributeValue = await _productAttributeValueRepository.GetAll(CancellationToken);

            //Assert
            Assert.Equal(2, newProductAttributeValue.Count());
        }
    }
}
