using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Prices
{
    public class PriceAddtests : BaseTests
    {
        private readonly IPriceRepository _priceRepository;

        public PriceAddtests()
        {
            _priceRepository = new PriceRepository(DbContext);
        }

        [Fact]
        public async void Add_AddNewEntity_ReturnsSameEntity()
        {
            //Arrange
            int id = 1;
            Price expectedPrice = new()
            {
                Id = id,
                Amount = 1,
                MaxQuantity = 3,
                Grade = Grade.عالی,
                ProductId = 1,
            };

            //Act
            _priceRepository.Add(expectedPrice);
            await UnitOfWork.SaveAsync(CancellationToken);
            Price actualPrice = DbContext.Prices.Where(p => p.Id == id).First();

            //Assert
            Assert.Equal(expectedPrice.Id, actualPrice.Id);
            Assert.Equal(expectedPrice.Amount, actualPrice.Amount);
            Assert.Equal(expectedPrice.MaxQuantity, actualPrice.MaxQuantity);
            Assert.Equal(expectedPrice.Grade, actualPrice.Grade);
            Assert.Equal(expectedPrice.ProductId, actualPrice.ProductId);
        }

        [Fact]
        public void Add_AddNullEntity_ReturnsException()
        {
            //Arrange
            Price price = new();

            //Assert
            var ex = Assert.Throws<DbUpdateException>(() => _priceRepository.Add(price));
        }

        [Fact]
        public void Add_AddNewNegativeEntityAmount_ReturnsException()
        {
            //Arrange
            int id = 1;
            Price price = new()
            {
                Id = id,
                Amount = -1,
                MaxQuantity = 1,
                Grade = Grade.عالی,
                ProductId = 5
            };

            //Assert
            Assert.Throws<DbUpdateException>(() => _priceRepository.Add(price));
        }

        [Fact]
        public void Add_AddNewNegativeEntityMaxQuantity_ReturnsException()
        {
            //Arrange
            int id = 1;
            Price price = new()
            {
                Id = id,
                Amount = 1,
                MaxQuantity = -1,
                Grade = Grade.عالی,
                ProductId = 5
            };

            //Assert
            Assert.Throws<DbUpdateException>(() => _priceRepository.Add(price));
        }

        [Fact]
        public void Add_AddNewEntityWithoutProductId_ReturnsException()
        {
            //Arrange
            int id = 1;
            Price price = new()
            {
                Id = id,
                Amount = 1,
                MaxQuantity = 1,
                Grade = Grade.عالی,
            };

            //Assert
            Assert.Throws<DbUpdateException>(() => _priceRepository.Add(price));
        }

        [Fact]
        public void Add_AddNewWrongGrade_ReturnsException()
        {
            //Arrange
            int id = 1;
            Price price = new()
            {
                Id = id,
                Amount = 1,
                MaxQuantity = 1,
                Grade = 0,
                ProductId = 5,
            };

            //Assert
            Assert.Throws<DbUpdateException>(() => _priceRepository.Add(price));
        }

        [Fact]
        public async Task AddAsync_AddNewEntity_ReturnsSameEntity()
        {
            //Arrange
            int id = 1;
            Price expectedPrice = new()
            {
                Id = id,
                Amount = 1,
                MaxQuantity = 3,
                Grade = Grade.عالی,
                ProductId = 1,
            };

            //Act
            await _priceRepository.AddAsync(expectedPrice, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
            Price actualPrice = DbContext.Prices.Where(p => p.Id == id).First();

            //Assert
            Assert.Equal(expectedPrice.Id, actualPrice.Id);
            Assert.Equal(expectedPrice.Amount, actualPrice.Amount);
            Assert.Equal(expectedPrice.MaxQuantity, actualPrice.MaxQuantity);
            Assert.Equal(expectedPrice.Grade, actualPrice.Grade);
            Assert.Equal(expectedPrice.ProductId, actualPrice.ProductId);
        }

        [Fact]
        public async Task AddAsync_AddNullEntity_ReturnsException()
        {
            //Arrange
            Price price = new();

            //Assert
            var ex = await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.AddAsync(price, CancellationToken));
        }

        [Fact]
        public async Task AddAsync_AddNewNegativeEntityAmount_ReturnsException()
        {
            //Arrange
            int id = 1;
            Price price = new()
            {
                Id = id,
                Amount = -1,
                MaxQuantity = 1,
                Grade = Grade.عالی,
                ProductId = 5
            };

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.AddAsync(price, CancellationToken));
        }

        [Fact]
        public async Task AddAsync_AddNewNegativeEntityMaxQuantity_ReturnsException()
        {
            //Arrange
            int id = 1;
            Price price = new()
            {
                Id = id,
                Amount = 1,
                MaxQuantity = -1,
                Grade = Grade.عالی,
                ProductId = 5
            };

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.AddAsync(price, CancellationToken));
        }

        [Fact]
        public async Task AddAsync_AddNewWrongGrade_ReturnsException()
        {
            //Arrange
            int id = 1;
            Price price = new()
            {
                Id = id,
                Amount = 1,
                MaxQuantity = 1,
                Grade = 0,
                ProductId = 5,
            };

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.AddAsync(price, CancellationToken));
        }

        [Fact]
        public async Task AddAll_AddAllEntities_ReturnsThreeEntities()
        {
            //Arrange
            int expectedCount = 3;
            List<Price> price =
            [
                new Price()
                {
                    Id = 1,
                    Amount = 2
                },
                new Price()
                {
                    Id = 2,
                    Amount = 3
                },
                new Price()
                {
                    Id = 3,
                    Amount = 4
                }
            ];

            //Act
            _ = _priceRepository.AddAll(price, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualPrices = await _priceRepository.GetAll(CancellationToken);
            int actualCount = actualPrices.Count();

            //Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task AddAll_AddAllNullEntities_ReturnsException()
        {
            //Arrange
            List<Price> price =
            [
                new Price()
                {
                },
                new Price()
                {
                },
                new Price()
                {
                }
            ];

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.AddAll(price, CancellationToken));
        }

        [Fact]
        public async Task AddAll_AddAllNegativeAmount_ReturnsException()
        {
            //Arrange
            List<Price> price =
            [
                new Price()
                {
                    Id = 1,
                    Amount = -2
                },
                new Price()
                {
                    Id = 2,
                    Amount = -3
                },
                new Price()
                {
                    Id = 3,
                    Amount = 4
                }
            ];

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.AddAll(price, CancellationToken));
        }

        [Fact]
        public async Task AddAll_AddAllNegativeMaxQuantity_ReturnsException()
        {
            //Arrange
            List<Price> price =
            [
                new Price()
                {
                    Id = 1,
                    Amount = 2,
                    MaxQuantity = -3
                },
                new Price()
                {
                    Id = 2,
                    Amount = 3,
                    MaxQuantity = -4
                },
                new Price()
                {
                    Id = 3,
                    Amount = 4,
                    MaxQuantity = -5
                }
            ];

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.AddAll(price, CancellationToken));
        }

        [Fact]
        public async Task AddAll_AddAllWrongGrade_ReturnsException()
        {
            //Arrange
            List<Price> price =
            [
                new Price()
                {
                    Id = 1,
                    Amount = 2,
                    MaxQuantity = -3,
                    Grade = 0
                },
                new Price()
                {
                    Id = 2,
                    Amount = 3,
                    MaxQuantity = -4,
                    Grade = 0
                },
                new Price()
                {
                    Id = 3,
                    Amount = 4,
                    MaxQuantity = -5,
                    Grade = 0
                }
            ];

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.AddAll(price, CancellationToken));
        }

        [Fact]
        public async Task AddRange_AddAllEntities_ReturnsThreeEntities()
        {
            //Arrange
            int expectedCount = 3;
            List<Price> price =
            [
                new Price()
                {
                    Id = 1,
                    Amount = 2
                },
                new Price()
                {
                    Id = 2,
                    Amount = 3
                },
                new Price()
                {
                    Id = 3,
                    Amount = 4
                }
            ];

            //Act
            _priceRepository.AddRange(price);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualPrices = await _priceRepository.GetAll(CancellationToken);
            int actualCount = actualPrices.Count();

            //Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void AddRange_AddAllNullEntities_ReturnsException()
        {
            //Arrange
            List<Price> price =
            [
                new Price()
                {
                },
                new Price()
                {
                },
                new Price()
                {
                }
            ];

            //Assert
            Assert.Throws<DbUpdateException>(() => _priceRepository.AddRange(price));
        }

        [Fact]
        public void AddRange_AddAllNegativeAmount_ReturnsException()
        {
            //Arrange
            List<Price> price =
            [
                new Price()
                {
                    Id = 1,
                    Amount = -2
                },
                new Price()
                {
                    Id = 2,
                    Amount = -3
                },
                new Price()
                {
                    Id = 3,
                    Amount = 4
                }
            ];

            //Assert
            Assert.Throws<DbUpdateException>(() => _priceRepository.AddRange(price));
        }

        [Fact]
        public void AddRange_AddAllNegativeMaxQuantity_ReturnsException()
        {
            //Arrange
            List<Price> price =
            [
                new Price()
                {
                    Id = 1,
                    Amount = 2,
                    MaxQuantity = -3
                },
                new Price()
                {
                    Id = 2,
                    Amount = 3,
                    MaxQuantity = -4
                },
                new Price()
                {
                    Id = 3,
                    Amount = 4,
                    MaxQuantity = -5
                }
            ];

            //Assert
            Assert.Throws<DbUpdateException>(() => _priceRepository.AddRange(price));
        }

        [Fact]
        public void AddRange_AddAllWrongGrade_ReturnsException()
        {
            //Arrange
            List<Price> price =
            [
                new Price()
                {
                    Id = 1,
                    Amount = 2,
                    MaxQuantity = -3,
                    Grade = 0
                },
                new Price()
                {
                    Id = 2,
                    Amount = 3,
                    MaxQuantity = -4,
                    Grade = 0
                },
                new Price()
                {
                    Id = 3,
                    Amount = 4,
                    MaxQuantity = -5,
                    Grade = 0
                }
            ];

            //Assert
            Assert.Throws<DbUpdateException>(() => _priceRepository.AddRange(price));
        }

        [Fact]
        public async Task AddRangeAsync_AddAllEntities_ReturnsThreeEntities()
        {
            //Arrange
            int expectedCount = 3;
            List<Price> price =
            [
                new Price()
                {
                    Id = 1,
                    Amount = 2
                },
                new Price()
                {
                    Id = 2,
                    Amount = 3
                },
                new Price()
                {
                    Id = 3,
                    Amount = 4
                }
            ];

            //Act
            await _priceRepository.AddRangeAsync(price, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualPrices = await _priceRepository.GetAll(CancellationToken);
            int actualCount = actualPrices.Count();

            //Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task AddRangeAsync_AddAllNullEntities_ReturnsException()
        {
            //Arrange
            List<Price> price =
            [
                new Price()
                {
                },
                new Price()
                {
                },
                new Price()
                {
                }
            ];

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.AddRangeAsync(price, CancellationToken));
        }

        [Fact]
        public async Task AddRangeAsync_AddAllNegativeAmount_ReturnsException()
        {
            //Arrange
            List<Price> price =
            [
                new Price()
                {
                    Id = 1,
                    Amount = -2
                },
                new Price()
                {
                    Id = 2,
                    Amount = -3
                },
                new Price()
                {
                    Id = 3,
                    Amount = 4
                }
            ];

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.AddRangeAsync(price, CancellationToken));
        }

        [Fact]
        public async Task AddRangeAsync_AddAllNegativeMaxQuantity_ReturnsException()
        {
            //Arrange
            List<Price> price =
            [
                new Price()
                {
                    Id = 1,
                    Amount = 2,
                    MaxQuantity = -3
                },
                new Price()
                {
                    Id = 2,
                    Amount = 3,
                    MaxQuantity = -4
                },
                new Price()
                {
                    Id = 3,
                    Amount = 4,
                    MaxQuantity = -5
                }
            ];

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.AddRangeAsync(price, CancellationToken));
        }

        [Fact]
        public async Task AddRangeAsync_AddAllWrongGrade_ReturnsException()
        {
            //Arrange
            List<Price> price =
            [
                new Price()
                {
                    Id = 1,
                    Amount = 2,
                    MaxQuantity = -3,
                    Grade = 0
                },
                new Price()
                {
                    Id = 2,
                    Amount = 3,
                    MaxQuantity = -4,
                    Grade = 0
                },
                new Price()
                {
                    Id = 3,
                    Amount = 4,
                    MaxQuantity = -5,
                    Grade = 0
                }
            ];

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _priceRepository.AddRangeAsync(price, CancellationToken));
        }


    }
}
