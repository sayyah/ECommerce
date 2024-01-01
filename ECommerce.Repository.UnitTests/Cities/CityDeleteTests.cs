using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.Cities
{
    public class CityDeleteTests : BaseTests
    {
        private readonly ICityRepository _cityRepository;

        public CityDeleteTests()
        {
            _cityRepository = new CityRepository(DbContext);
        }

        [Fact]
        public async Task Delete_DeleteEntity_ReturnsZeroCount()
        {
            //Arrange
            int id = 1000
                , expectedCount = 0;
            City city = new()
            {
                Id = id,
                Name = "رشت",
                StateId = 3,
            };
            DbContext.Cities.Add(city);
            DbContext.SaveChanges();

            //Act
            _cityRepository.Delete(city);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualCount = DbContext.Cities.Count();

            //Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task DeleteRange_DeleteEntities_ReturnsZeroCount()
        {
            //Arrange
            int expectedCount = 0;
            List<City> city =
            [
                new City()
                {
                    Id = 1,
                    Name = "رشت",
                    StateId = 3,
                },
                new City()
                {
                    Id = 2,
                    Name = "تهران",
                    StateId = 4,
                },
                new City()
                {
                    Id = 3,
                    Name = "قزوین",
                    StateId = 5,
                }
            ];
            DbContext.Cities.AddRange(city);
            DbContext.SaveChanges();

            //Act
            _cityRepository.DeleteRange(city);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualCount = DbContext.Cities.Count();

            //Assert
            Assert.Equal(expectedCount, actualCount);
        }
    }
}
