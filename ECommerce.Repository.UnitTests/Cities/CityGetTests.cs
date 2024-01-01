using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Cities
{
    public class CityGetTests : BaseTests
    {
        private readonly ICityRepository _cityRepository;

        public CityGetTests()
        {
            _cityRepository = new CityRepository(DbContext);
        }

        [Fact]
        public async Task GetByIdAsync_GetEntityById_ReturnsTrueEntity()
        {
            //Arrange
            int id = 1000;
            City expectedCity = new()
            {
                Id = id,
                Name = "رشت",
                StateId = 2,
            };
            DbContext.Cities.Add(expectedCity);
            DbContext.SaveChanges();

            //Act
            var actualCity = await _cityRepository.GetByIdAsync(CancellationToken, id);

            //Assert
            Assert.Equal(expectedCity.Id, actualCity.Id);
            Assert.Equal(expectedCity.Name, actualCity.Name);
        }

        [Fact]
        public async Task GetByIdAsync_GetNotExistEntityById_ReturnsException()
        {
            //Arrange
            int wrongId = 1005;
            City expectedCity = new()
            {
                Id = 1000,
                Name = "رشت",
                StateId = 2
            };
            DbContext.Cities.Add(expectedCity);
            DbContext.SaveChanges();

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _cityRepository.GetByIdAsync(CancellationToken, wrongId));
        }

        [Fact]
        public async Task GetByName_GetEntityByName_ReturnsTrueEntitiy()
        {
            //Arrange
            string expectedCity = "رشت";
            City city = new()
            {
                Id = 1000,
                Name = expectedCity,
                StateId = 2,
            };
            DbContext.Cities.Add(city);
            DbContext.SaveChanges();

            //Act
            var actualCity = await _cityRepository.GetByName(expectedCity, CancellationToken);

            //Assert
            Assert.Equal(expectedCity, actualCity.Name);
        }


        [Fact]
        public async Task GetAllAsync_GetAllEntities_ReturnsTrueEntities()
        {
            //Arrange
            List<City> expectedCities =
            [
                new City()
                {
                    Id = 1000,
                    Name = "رشت",
                    StateId = 3,
                },
                new City()
                {
                    Id = 10001,
                    Name = "تهران",
                    StateId = 4,
                },
            ];
            DbContext.Cities.AddRange(expectedCities);
            DbContext.SaveChanges();

            //Act
            var getCities = await _cityRepository.GetAllAsync(CancellationToken);
            var actualCities = getCities.ToList();

            //Assert
            Assert.Equal(expectedCities[0].Id, actualCities[0].Id);
            Assert.Equal(expectedCities[0].Name, actualCities[0].Name);
            Assert.Equal(expectedCities[0].StateId, actualCities[0].StateId);
            Assert.Equal(expectedCities[1].Id, actualCities[1].Id);
            Assert.Equal(expectedCities[1].Name, actualCities[1].Name);
            Assert.Equal(expectedCities[1].StateId, actualCities[1].StateId);
        }

    }
}
