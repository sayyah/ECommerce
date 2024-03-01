using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Prices
{
    public class PriceUpdateTests : BaseTests
    {
        private readonly IPriceRepository _priceRepository;

        public PriceUpdateTests()
        {
            _priceRepository = new PriceRepository(DbContext);
        }

        [Fact]
        public async void Update_UpdateAmount_ReturnUpdatedAmount()
        {
            //Arrange
            int id = 1;
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
            price.Amount = 2;

            //Act
            _priceRepository.Update(price);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualPrice = DbContext.Prices.Where(c => c.Id == id).First();

            //Assert
            Assert.Equal(price.Amount, actualPrice.Amount);
        }

        [Fact]
        public void Update_UpdateToNegativeAmount_ReturnsException()
        {
            //Arrange
            int id = 1;
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
            price.Amount = -2;

            //Assert
            Assert.Throws<DbUpdateException>(() => _priceRepository.Update(price));
        }

        [Fact]
        public void Update_UpdateToNegativeMaxQuantity_ReturnsException()
        {
            //Arrange
            int id = 1;
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
            price.MaxQuantity = -2;

            //Assert
            Assert.Throws<DbUpdateException>(() => _priceRepository.Update(price));
        }

        [Fact]
        public void Update_UpdateToWrongGrade_ReturnsException()
        {
            //Arrange
            int id = 1;
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
            price.Grade = 0;

            //Assert
            Assert.Throws<DbUpdateException>(() => _priceRepository.Update(price));
        }

        [Fact]
        public async Task UpdateAsync_UpdateAmount_ReturnUpdatedAmount()
        {
            //Arrange
            int id = 1;
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
            price.Amount = 2;

            //Act
            await _priceRepository.UpdateAsync(price, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualPrice = DbContext.Prices.Where(c => c.Id == id).First();

            //Assert
            Assert.Equal(price.Amount, actualPrice.Amount);
        }

        [Fact]
        public async Task UpdateAsync_UpdateToNegativeAmount_ReturnsException()
        {
            //Arrange
            int id = 1;
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
            price.Amount = -2;

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.UpdateAsync(price, CancellationToken));
        }

        [Fact]
        public async Task UpdateAsync_UpdateToNegativeMaxQuantity_ReturnsException()
        {
            //Arrange
            int id = 1;
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
            price.MaxQuantity = -2;

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.UpdateAsync(price, CancellationToken));
        }

        [Fact]
        public async Task UpdateAsync_UpdateToWrongGrade_ReturnsException()
        {
            //Arrange
            int id = 1;
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
            price.Grade = 0;

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.UpdateAsync(price, CancellationToken));
        }

        [Fact]
        public async Task UpdateRange_UpdateAmounts_ReturnUpdatedAmounts()
        {
            //Arrange
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
            price[0].Amount = 2;
            price[1].Amount = 3;
            price[2].Amount = 4;

            //Act
            _priceRepository.UpdateRange(price);
            await UnitOfWork.SaveAsync(CancellationToken);
            var getPrices = await _priceRepository.GetAll(CancellationToken);
            var actualPrices = getPrices.ToList();

            //Assert
            Assert.Equal(price[0].Amount, actualPrices[0].Amount);
            Assert.Equal(price[1].Amount, actualPrices[1].Amount);
            Assert.Equal(price[2].Amount, actualPrices[2].Amount);
        }

        [Fact]
        public void UpdateRange_UpdateToNegativeAmounts_ReturnsException()
        {
            //Arrange
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
            price[0].Amount = -2;
            price[1].Amount = -3;
            price[2].Amount = 4;

            //Assert
            Assert.Throws<DbUpdateException>(() => _priceRepository.UpdateRange(price));
        }

        [Fact]
        public void UpdateRange_UpdateToNegativeMaxQuantities_ReturnsException()
        {
            //Arrange
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
            price[0].MaxQuantity = -2;
            price[1].MaxQuantity = -3;
            price[2].MaxQuantity = 4;

            //Assert
            Assert.Throws<DbUpdateException>(() => _priceRepository.UpdateRange(price));
        }

        [Fact]
        public void UpdateRange_UpdateToWrongGrades_ReturnsException()
        {
            //Arrange
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
            price[0].Grade = 0;
            price[1].Grade = Grade.عالی;
            price[2].Grade = 0;

            //Assert
            Assert.Throws<DbUpdateException>(() => _priceRepository.UpdateRange(price));
        }

        [Fact]
        public async Task UpdateRangeAsync_UpdateAmounts_ReturnUpdatedAmounts()
        {
            //Arrange
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
            price[0].Amount = 2;
            price[1].Amount = 3;
            price[2].Amount = 4;

            //Act
            await _priceRepository.UpdateRangeAsync(price, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
            var getPrices = await _priceRepository.GetAll(CancellationToken);
            var actualPrices = getPrices.ToList();

            //Assert
            Assert.Equal(price[0].Amount, actualPrices[0].Amount);
            Assert.Equal(price[1].Amount, actualPrices[1].Amount);
            Assert.Equal(price[2].Amount, actualPrices[2].Amount);
        }

        [Fact]
        public async Task UpdateRangeAsync_UpdateToNegativeAmounts_ReturnsException()
        {
            //Arrange
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
            price[0].Amount = -2;
            price[1].Amount = -3;
            price[2].Amount = 4;

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.UpdateRangeAsync(price, CancellationToken));
        }

        [Fact]
        public async Task UpdateRangeAsync_UpdateToNegativeMaxQuantities_ReturnsException()
        {
            //Arrange
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
            price[0].MaxQuantity = -2;
            price[1].MaxQuantity = -3;
            price[2].MaxQuantity = 4;

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.UpdateRangeAsync(price, CancellationToken));
        }

        [Fact]
        public async Task UpdateRangeAsync_UpdateToWrongGrades_ReturnsException()
        {
            //Arraneg
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
            price[0].Grade = 0;
            price[1].Grade = Grade.عالی;
            price[2].Grade = 0;

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.UpdateRangeAsync(price, CancellationToken));
        }
    }
}
