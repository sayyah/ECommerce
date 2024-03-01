using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.Prices
{
    public class PriceDeletetests : BaseTests
    {
        private readonly IPriceRepository _priceRepository;

        public PriceDeletetests()
        {
            _priceRepository = new PriceRepository(DbContext);
        }

        [Fact]
        public async void Delete_DeleteEntity_ReturnsZeroCount()
        {
            //Arrange
            int id = 1
                , expectedCount = 0;
            Price price = new()
            {
                Id = id,
                Amount = 1,
                MaxQuantity = 3,
                Grade = Grade.عالی,
                ProductId = 5
            };
            DbContext.Prices.Add(price);
            DbContext.SaveChanges();

            //Act
            _priceRepository.Delete(price);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualCount = DbContext.Prices.Count();

            //Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task DeleteAsync_DeleteEntity_ReturnsZeroCount()
        {
            //Arrange
            int id = 1
                , expectedCount = 0;
            Price price = new()
            {
                Id = id,
                Amount = 1,
                MaxQuantity = 3,
                Grade = Grade.عالی,
                ProductId = 5
            };
            DbContext.Prices.Add(price);
            DbContext.SaveChanges();

            //Act
            await _priceRepository.DeleteAsync(price, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualCount = DbContext.Prices.Count();

            //Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task DeleteAsync_DeleteEntityById_ReturnsZeroCount()
        {
            //Arrange
            int id = 1
                , expectedCount = 0;
            Price price = new()
            {
                Id = id,
                Amount = 1,
                MaxQuantity = 3,
                Grade = Grade.عالی,
                ProductId = 5
            };
            DbContext.Prices.Add(price);
            DbContext.SaveChanges();

            //Act
            await _priceRepository.DeleteAsync(price.Id, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualCount = DbContext.Prices.Count();

            //Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async void DeleteRange_DeleteEntities_ReturnsZeroCount()
        {
            //Arrange
            int expectedCount = 0;
            List<Price> price =
            [
                new Price()
                {
                    Id = 1,
                    Amount = 2,
                    MaxQuantity = 3,
                    Grade = Grade.عالی,
                    ProductId = 5
                },
                new Price()
                {
                    Id = 2,
                    Amount = 3,
                    MaxQuantity = 3,
                    Grade = Grade.عالی,
                    ProductId = 5
                },
                new Price()
                {
                    Id = 3,
                    Amount = 4,
                    MaxQuantity = 3,
                    Grade = Grade.عالی,
                    ProductId = 5
                }
            ];
            DbContext.Prices.AddRange(price);
            DbContext.SaveChanges();

            //Act
            _priceRepository.DeleteRange(price);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualCount = DbContext.Prices.Count();

            //Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task DeleteRangeAsync_DeleteEntities_ReturnsZeroCount()
        {
            //Arrange
            int expectedCount = 0;
            List<Price> price =
            [
                new Price()
                {
                    Id = 1,
                    Amount = 2,
                    MaxQuantity = 3,
                    Grade = Grade.عالی,
                    ProductId = 5
                },
                new Price()
                {
                    Id = 2,
                    Amount = 3,
                    MaxQuantity = 3,
                    Grade = Grade.عالی,
                    ProductId = 5
                },
                new Price()
                {
                    Id = 3,
                    Amount = 4,
                    MaxQuantity = 3,
                    Grade = Grade.عالی,
                    ProductId = 5
                }
            ];
            DbContext.Prices.AddRange(price);
            DbContext.SaveChanges();

            //Act
            await _priceRepository.DeleteRangeAsync(price, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualCount = DbContext.Prices.Count();

            //Assert
            Assert.Equal(expectedCount, actualCount);
        }

    }
}



