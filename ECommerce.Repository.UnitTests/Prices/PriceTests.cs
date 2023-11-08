using Ecommerce.Entities;
using Ecommerce.Entities.Helper;
using ECommerce.API.Interface;
using ECommerce.API.Repository;
using ECommerce.Repository.UnitTests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Repository.UnitTests.Prices
{
    public class PriceTests : BaseTests
    {
        private readonly IPriceRepository _priceRepository;

        public PriceTests()
        {
            _priceRepository = new PriceRepository(DbContext);
        }

        [Fact]
        public async Task AddAsync_AddNewEntity_ReturnsSameEntity()
        {
            //Arrange
            int id = 2;
            int amount = 3;
            //Grade grade = Grade.عالی;
            Price price = new Price
            {
                Id = id,
                Amount = amount,
                MaxQuantity = 0
            };

            //Act
            Price newPrice = await _priceRepository.AddAsync(price, CancellationToken);

            //Assert
            Assert.Equal(id, newPrice.Id);
            Assert.Equal(amount, newPrice.Amount);
            //Assert.Equal(grade, newPrice.Grade);
        }
    }
}
