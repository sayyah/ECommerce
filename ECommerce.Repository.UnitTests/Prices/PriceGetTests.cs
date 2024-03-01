using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Prices
{
    public class PriceGetTests : BaseTests
    {
        private readonly IPriceRepository _priceRepository;

        public PriceGetTests()
        {
            _priceRepository = new PriceRepository(DbContext);
        }

        [Fact]
        public void GetById_GetEntityById_ReturnsTrueEntitiy()
        {
            //Arrange
            int id = 1;
            Price expectedPrice = new Price()
            {
                Id = id,
                Amount = 2,
                MaxQuantity = 3,
                ProductId = 4
            };
            DbContext.Prices.Add(expectedPrice);
            DbContext.SaveChanges();

            //Act
            var actualPrice = _priceRepository.GetById(id);

            //Assert
            Assert.Equal(expectedPrice.Id, actualPrice.Id);
            Assert.Equal(expectedPrice.Amount, actualPrice.Amount);
            Assert.Equal(expectedPrice.MaxQuantity, actualPrice.MaxQuantity);
            Assert.Equal(expectedPrice.ProductId, actualPrice.ProductId);
        }

        [Fact]
        public void GetById_GetNotExistEntityById_ReturnsException()
        {
            //Arrange
            int wrongId = 2;
            Price expectedPrice = new Price()
            {
                Id = 1,
                Amount = 2,
                MaxQuantity = 3,
                ProductId = 4
            };
            DbContext.Prices.Add(expectedPrice);
            DbContext.SaveChanges();

            //Assert
            Assert.Throws<DbUpdateException>(() => _priceRepository.GetById(wrongId));
        }


        [Fact]
        public async Task GetByIdAsync_GetEntityById_ReturnsTrueEntity()
        {
            //Arrange
            int id = 1;
            Price expectedPrice = new Price()
            {
                Id = id,
                Amount = 2,
                MaxQuantity = 3,
                ProductId = 4
            };
            DbContext.Prices.Add(expectedPrice);
            DbContext.SaveChanges();

            //Act
            var actualPrice = await _priceRepository.GetByIdAsync(CancellationToken, id);

            //Assert
            Assert.Equal(expectedPrice.Id, actualPrice.Id);
            Assert.Equal(expectedPrice.Amount, actualPrice.Amount);
            Assert.Equal(expectedPrice.MaxQuantity, actualPrice.MaxQuantity);
            Assert.Equal(expectedPrice.ProductId, actualPrice.ProductId);
        }

        [Fact]
        public async Task GetByIdAsync_GetNotExistEntityById_ReturnsException()
        {
            //Arrange
            int wrongId = 2;
            Price expectedPrice = new Price()
            {
                Id = 1,
                Amount = 2,
                MaxQuantity = 3,
                ProductId = 4
            };
            DbContext.Prices.Add(expectedPrice);
            DbContext.SaveChanges();

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.GetByIdAsync(CancellationToken, wrongId));
        }


        [Fact]
        public async Task GetAll_GetAllEntities_ReturnsTrueEntities()
        {
            //Arrange
            int id1 = 1, 
                id2 = 2;
            List<Price> expectedPrice =
            [
                new Price()
                {
                    Id = id1,
                    Amount = 2,
                    MaxQuantity = 3,
                    ProductId = 4
                },
                new Price()
                {
                    Id = id2,
                    Amount = 2,
                    MaxQuantity = 3,
                    ProductId = 4
                },
            ];
            DbContext.Prices.AddRange(expectedPrice);
            DbContext.SaveChanges();

            //Act
            var getPrices = await _priceRepository.GetAll(CancellationToken);
            var actualPrices = getPrices.ToList();

            //Assert
            Assert.Equal(expectedPrice[0].Id, actualPrices[0].Id);
            Assert.Equal(expectedPrice[0].Amount, actualPrices[0].Amount);
            Assert.Equal(expectedPrice[0].MaxQuantity, actualPrices[0].MaxQuantity);
            Assert.Equal(expectedPrice[0].ProductId, actualPrices[0].ProductId);
            Assert.Equal(expectedPrice[1].Id, actualPrices[1].Id);
            Assert.Equal(expectedPrice[1].Amount, actualPrices[1].Amount);
            Assert.Equal(expectedPrice[1].MaxQuantity, actualPrices[1].MaxQuantity);
            Assert.Equal(expectedPrice[1].ProductId, actualPrices[1].ProductId);
        }

    }
}
