using ECommerce.Domain.Entities.Helper;
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

namespace ECommerce.Repository.UnitTests.Cities
{
    public class CityUpdateTests : BaseTests
    {
        private readonly ICityRepository _cityRepository;

        public CityUpdateTests()
        {
            _cityRepository = new CityRepository(DbContext);
        }

        [Fact]
        public async Task Update_UpdateName_ReturnUpdatedName()
        {
            //Arrange
            int id = 1000;
            City city = new()
            {
                Id = id,
                Name = "رشت",
                StateId = 3,
            };
            DbContext.Cities.Add(city);
            DbContext.SaveChanges();
            city.Name = "تهران";

            //Act
            _cityRepository.Update(city);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualCity = DbContext.Cities.Where(c => c.Id == id).First();

            //Assert
            Assert.Equal(city.Name, actualCity.Name);
        }

        [Fact]
        public void Update_UpdateToEmptyName_ReturnsException()
        {
            //Arrange
            int id = 1000;
            City city = new()
            {
                Id = id,
                Name = "رشت",
                StateId = 3,
            };
            DbContext.Cities.Add(city);
            DbContext.SaveChanges();
            city.Name = "";

            //Assert
            Assert.Throws<DbUpdateException>(() => _cityRepository.Update(city));
        }

        [Fact]
        public async Task UpdateRange_UpdateNames_ReturnUpdatedNames()
        {
            //Arrange
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
                    Name = "رشت",
                    StateId = 3,
                },
                new City()
                {
                    Id = 1002,
                    Name = "رشت",
                    StateId = 3,
                }
            ];
            DbContext.Cities.AddRange(city);
            DbContext.SaveChanges();
            city[0].Name = "مشهد";
            city[1].Name = "شیراز";
            city[2].Name = "کرمان";

            //Act
            _cityRepository.UpdateRange(city);
            await UnitOfWork.SaveAsync(CancellationToken);
            var actualPrices = DbContext.Cities.ToList();

            //Assert
            Assert.Equal(city[0].Name, actualPrices[0].Name);
            Assert.Equal(city[1].Name, actualPrices[1].Name);
            Assert.Equal(city[2].Name, actualPrices[2].Name);
        }

        [Fact]
        public void UpdateRange_UpdateToEmptyNames_ReturnsException()
        {
            //Arrange
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
                    Name = "رشت",
                    StateId = 3,
                },
                new City()
                {
                    Id = 3,
                    Name = "رشت",
                    StateId = 3,
                }
            ];
            DbContext.Cities.AddRange(city);
            DbContext.SaveChanges();
            city[0].Name = "";
            city[1].Name = "";
            city[2].Name = "";

            //Assert
            Assert.Throws<DbUpdateException>(() => _cityRepository.UpdateRange(city));
        }
    }
}
