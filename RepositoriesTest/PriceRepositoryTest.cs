using API.DataContext;
using API.Repository;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesTest
{
    public class PriceRepositoryTest
    {
        public SunflowerECommerceDbContext context;
        public CancellationToken cancellationToken = new CancellationToken();
        public PriceRepository repository;
        public IEnumerable<Price?> prices;
        public PriceRepositoryTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SunflowerECommerceDbContext>()
                .EnableSensitiveDataLogging(true)
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            context = new SunflowerECommerceDbContext(optionsBuilder.Options);
            repository = new PriceRepository(context);
            prices = Setup();
            AddRang();
        }

        public async void AddRang()
        {
            prices = Setup();
            await repository.AddRangeAsync(prices, cancellationToken);
        }

        [Fact]
        public async void AddPriceReturnPrice()
        {
            //Arrange
            Price price=new Price();
            price.Id = 5; price.Amount = 0; price.MinQuantity = 1; price.MaxQuantity = 0; price.IsColleague = false;
            price.SellNumber = Price.HolooSellNumber.Sel_Price; price.ProductId = 3; price.Exist = 5; price.IsDefault = false;
            price.Grade = Entities.Helper.Grade.یک; price.DiscountId = 2;

            //Act
            var result=await repository.AddAsync(price,cancellationToken);

            //Assert
            Assert.True(price.Id==result.Id);
        }

        [Fact]
        public async void GetPriceByIdUpdateAmount()
        {
            //Arrange
            decimal newAmount = 530;
            var result = repository.GetById(2);
            result.Amount = newAmount;

            //Act             
            await repository.UpdateAsync(result,cancellationToken);
            var newResultAfterUpdate = repository.GetById(2);

            //Assert
            Assert.True(newResultAfterUpdate.Amount==newAmount);
        }

        [Fact]
        public async void GetPriceByIdDeletePrice()
        {
            //Arrange
            var result = repository.GetById(2);

            //Act             
            await repository.DeleteAsync(result, cancellationToken);
            var newResultAfterDelete = repository.GetById(2);

            //Assert
            Assert.Null(newResultAfterDelete);
        }



        public IEnumerable<Price?> Setup()
        {
            var Prices = new List<Price>()
            {
                new Price() { Id=1,  Amount=0,   MinQuantity=1,  MaxQuantity=0,  IsColleague=false,  SellNumber=Price.HolooSellNumber.Sel_Price,   
                    ProductId=3,    UnitId=1,   SizeId=1,   ColorId=6,  CurrencyId=1,   ArticleCode="204002",     ArticleCodeCustomer="1030000002",     
                    Exist=5,    IsDefault=false,    Grade=Entities.Helper.Grade.یک,    DiscountId=2 },
                new Price() { Id=2,  Amount=0,   MinQuantity=1,  MaxQuantity=0,  IsColleague=false,  SellNumber=Price.HolooSellNumber.Sel_Price,   
                    ProductId=4,    UnitId=1,   SizeId=1,   ColorId=2,  CurrencyId=1,   ArticleCode="204002",     ArticleCodeCustomer="1030000002",     
                    Exist=5,    IsDefault=false,    Grade=Entities.Helper.Grade.یک,    DiscountId=2 },
                new Price() { Id=3,  Amount=0,   MinQuantity=1,  MaxQuantity=0,  IsColleague=false,  SellNumber=Price.HolooSellNumber.Sel_Price,   
                    ProductId=5,    UnitId=1,   SizeId=1,   ColorId=6,  CurrencyId=1,   ArticleCode="204002",     ArticleCodeCustomer="1030000002",     
                    Exist=5,    IsDefault=false,    Grade=Entities.Helper.Grade.یک,    DiscountId=2 },
                new Price() { Id=4,  Amount=0,   MinQuantity=1,  MaxQuantity=0,  IsColleague=false,  SellNumber=Price.HolooSellNumber.Sel_Price,   
                    ProductId=3,    UnitId=1,   SizeId=1,   ColorId=2,  CurrencyId=1,   ArticleCode="204002",     ArticleCodeCustomer="1030000002",     
                    Exist=5,    IsDefault=false,    Grade=Entities.Helper.Grade.یک,    DiscountId=2 }

            };
            return Prices;
        }
    }
}
