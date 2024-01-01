using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Cities
{
    public class CityAddTests : BaseTests
    {
        private readonly ICityRepository _cityRepository;

        public CityAddTests()
        {
            _cityRepository = new CityRepository(DbContext);
        }

        [Fact]
        public async void Add_AddNewEntity_ReturnsSameEntity()
        {
            //Arrange
            int id = 1000;
            City expectedCity = new()
            {
                Id = id,
                Name = "رشت",
                StateId = 2,
            };

            //Act
            _cityRepository.Add(expectedCity);
            await UnitOfWork.SaveAsync(CancellationToken);
            City actualCity = DbContext.Cities.Where(p => p.Id == id).First();

            //Assert
            Assert.Equal(expectedCity.Id, actualCity.Id);
            Assert.Equal(expectedCity.Name, actualCity.Name);
            Assert.Equal(expectedCity.StateId, actualCity.StateId);
        }

        [Fact]
        public void Add_AddNullEntity_ReturnsException()
        {
            //Arrange
            City city = new();

            //Assert
            var ex = Assert.Throws<DbUpdateException>(() => _cityRepository.Add(city));
        }

        [Fact]
        public void Add_AddEmptyEntityName_ReturnsException()
        {
            //Arrange
            City city = new()
            {
                Id = 1000,
                Name = "",
                StateId = 2,
            };

            //Assert
            Assert.Throws<DbUpdateException>(() => _cityRepository.Add(city));
        }

        [Fact]
        public async Task AddAsync_AddNewEntity_ReturnsSameEntity()
        {
            //Arrange
            int id = 1000;
            City expectedCity = new()
            {
                Id = id,
                Name = "رشت",
                StateId = 2,
            };

            //Act
            await _cityRepository.AddAsync(expectedCity, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
            City actualCity = DbContext.Cities.Where(p => p.Id == id).First();

            //Assert
            Assert.Equal(expectedCity.Id, actualCity.Id);
            Assert.Equal(expectedCity.Name, actualCity.Name);
            Assert.Equal(expectedCity.StateId, actualCity.StateId);
        }

        [Fact]
        public async Task AddAsync_AddNullEntity_ReturnsException()
        {
            //Arrange
            City city = new();

            //Assert
            var ex = await Assert.ThrowsAsync<DbUpdateException>(() => _cityRepository.AddAsync(city, CancellationToken));
        }

        [Fact]
        public async Task AddAsync_AddNewEmptyEntityname_ReturnsException()
        {
            //Arrange
            City city = new()
            {
                Id = 1000,
                Name = "",
                StateId = 2,
            };

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _cityRepository.AddAsync(city, CancellationToken));
        }

        [Fact]
        public async Task AddRange_AddAllEntities_ReturnsThreeEntities()
        {
            //Arrange
            int expectedCount = 3;
            List<City> city =
            [
                new City()
                {
                    Id = 1000,
                    Name = "رشت",
                    StateId = 3,
                },
                new City()
                {
                    Id = 1001,
                    Name = "قزوین",
                    StateId = 4,
                },
                new City()
                {
                    Id = 1002,
                    Name = "تهران",
                    StateId = 5,
                }
            ];

            //Act
            _cityRepository.AddRange(city);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualCities = DbContext.Cities.ToList();

            //Assert
            Assert.Equal(expectedCount, actualCities.Count());
        }

        [Fact]
        public void AddRange_AddAllNullEntities_ReturnsException()
        {
            //Arrange
            List<City> city =
            [
                new City()
                {
                },
                new City()
                {
                },
                new City()
                {
                }
            ];

            //Assert
            Assert.Throws<DbUpdateException>(() => _cityRepository.AddRange(city));
        }

        [Fact]
        public async Task AddRangeAsync_AddAllEntities_ReturnsThreeEntities()
        {
            //Arrange
            int expectedCount = 3;
            List<City> city =
            [
                new City()
                {
                    Id = 1000,
                    Name = "رشت",
                    StateId = 3,
                },
                new City()
                {
                    Id = 1001,
                    Name = "قزوین",
                    StateId = 4,
                },
                new City()
                {
                    Id = 1002,
                    Name = "تهران",
                    StateId = 5,
                }
            ];

            //Act
            _cityRepository.AddRange(city);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualCities = DbContext.Cities.ToList();
            
            //Assert
            Assert.Equal(expectedCount, actualCities.Count());
        }
    }
}
