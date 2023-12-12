using AutoFixture;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.WishLists;

public class WishListTests : BaseTests
{
    private readonly IWishListRepository _wishListRepository;

    public WishListTests()
    {
        _wishListRepository = new WishListRepository(DbContext);
    }

    private User AddUser()
    {
        // User user = _fixture.Create<User>(); // recursion error
        User user =
            new()
            {
                Id = Fixture.Create<int>(),
                Mobile = "09123456789",
                FirstName = "John",
                LastName = "Doe"
            };
        DbContext.Users.Add(user);
        DbContext.SaveChanges();
        return user;
    }

    private Price AddPrice(Product product)
    {
        // Price price = _fixture.Create<Price>(); // recursion error
        Price price =
            new()
            {
                Id = Fixture.Create<int>(),
                Amount = Fixture.Create<decimal>(),
                Product = product
            };
        DbContext.Prices.Add(price);
        DbContext.SaveChanges();
        return price;
    }

    private Brand AddBrand()
    {
        Brand brand = new() { Id = Fixture.Create<int>(), Name = Fixture.Create<string>() };
        DbContext.Brands.Add(brand);
        DbContext.SaveChanges();
        return brand;
    }

    private Image AddImage()
    {
        Image image =
            new()
            {
                Id = Fixture.Create<int>(),
                Path = Fixture.Create<string>(),
                Name = Fixture.Create<string>(),
                Alt = Fixture.Create<string>()
            };
        DbContext.Images.Add(image);
        DbContext.SaveChanges();
        return image;
    }

    private (Product, Price) AddProduct()
    {
        var brand = AddBrand();
        var image = AddImage();
        Product product =
            new()
            {
                Id = 1,
                Name = "productName",
                Url = "productUrl",
                BrandId = 1,
                Brand = brand,
                Images = new List<Image> { image }
            };
        DbContext.Products.Add(product);
        DbContext.SaveChanges();
        var price = AddPrice(product);
        return (product, price);
    }

    [Fact]
    public async Task AddAsync_AddNewEntity_ReturnsSameEntity()
    {
        //Arrange
        var user = AddUser();
        var (_, price) = AddProduct();
        int id = 1;
        WishList wishList =
            new()
            {
                Id = id,
                UserId = user.Id,
                PriceId = price.Id,
            };

        //Act
        WishList newWishList = await _wishListRepository.AddAsync(wishList, CancellationToken);

        //Assert
        Assert.Equal(id, newWishList.Id);
        Assert.Equal(user.Id, newWishList.UserId);
        Assert.Equal(price.Id, newWishList.PriceId);
    }

    [Fact]
    public async Task DeleteAsync_DeleteOneEntity_ReturnDeletedEntity()
    {
        //Arrange
        var user = AddUser();
        var (_, price) = AddProduct();
        int id = 1;

        WishList wishList =
            new()
            {
                Id = id,
                UserId = user.Id,
                PriceId = price.Id,
            };

        //Act
        await DbContext.WishLists.AddAsync(wishList, CancellationToken);
        await DbContext.SaveChangesAsync();
        var newWishList = _wishListRepository.DeleteAsync(wishList.Id, CancellationToken);

        //Assert
        Assert.Equal(id, newWishList.Id);
    }

    [Fact]
    public async Task DeleteAsync_DeleteNullEntity_ReturnFaulted()
    {
        //Arrange
        var user = AddUser();
        var (_, price) = AddProduct();
        int falseId = 2;
        int id = 1;

        WishList wishList =
            new()
            {
                Id = id,
                UserId = user.Id,
                PriceId = price.Id,
            };

        //Act
        await DbContext.WishLists.AddAsync(wishList, CancellationToken);
        await DbContext.SaveChangesAsync();
        var newWishList = _wishListRepository.DeleteAsync(falseId, CancellationToken);

        //Assert
        Assert.Equal(TaskStatus.Faulted, newWishList.Status);
    }

    [Fact]
    public async Task EditAsync_EditNotExistEntity_ThrowException()
    {
        //Arrange
        var user = AddUser();
        var (_, price) = AddProduct();
        int falseId = 3;
        int id = 1;

        WishList wishList =
            new()
            {
                Id = id,
                UserId = user.Id,
                PriceId = price.Id,
            };

        WishList editWishList = new() { Id = falseId };

        //Act
        await DbContext.WishLists.AddAsync(wishList, CancellationToken);
        await DbContext.SaveChangesAsync();
        Task action() =>
            _wishListRepository.UpdateAsync(editWishList, CancellationToken.None, true);

        //Assert
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(action);
    }

    [Fact]
    public async Task GetAll_CountAllEntities_ReturnsTwoEntities()
    {
        //Arrange
        var user = AddUser();
        var (_, price) = AddProduct();
        int firstId = 1;
        int secondId = 2;

        WishList firstWishList =
            new()
            {
                Id = firstId,
                UserId = user.Id,
                PriceId = price.Id,
            };

        WishList secondWishList =
            new()
            {
                Id = secondId,
                UserId = user.Id,
                PriceId = price.Id,
            };

        await DbContext.WishLists.AddAsync(firstWishList, CancellationToken);
        await DbContext.WishLists.AddAsync(secondWishList, CancellationToken);
        await DbContext.SaveChangesAsync();

        //Act
        var newWishLists = await _wishListRepository.GetAll(CancellationToken);

        //Assert
        Assert.Equal(2, newWishLists.Count());
    }

    [Fact]
    public async Task GetByProductUser_AddNewEntity_ReturnsSameEntity()
    {
        //Arrange
        int id = 1;
        var user = AddUser();
        var (product, price) = AddProduct();

        WishList wishList =
            new()
            {
                Id = id,
                UserId = user.Id,
                PriceId = price.Id,
                Price = price
            };

        //Act
        await DbContext.WishLists.AddAsync(wishList, CancellationToken);
        await DbContext.SaveChangesAsync();
        var newWishList = await _wishListRepository.GetByProductUser(
            product.Id,
            user.Id,
            CancellationToken
        );

        //Assert
        Assert.Equal(id, newWishList.Id);
        Assert.Equal(price, newWishList.Price);
        Assert.Equal(product.Id, newWishList.Price!.ProductId);
    }

    [Fact]
    public async Task GetByIdWithInclude_AddTowEntity_ReturnsTwoViewModels()
    {
        //Arrange
        int firstId = 1;
        int secondId = 2;
        var user = AddUser();
        var (product, _) = AddProduct();

        WishList firstWishList =
            new()
            {
                Id = firstId,
                UserId = user.Id,
                PriceId = product.Prices!.FirstOrDefault()!.Id,
                Price = product.Prices!.FirstOrDefault()
            };

        WishList secondWishList =
            new()
            {
                Id = secondId,
                UserId = user.Id,
                PriceId = product.Prices!.FirstOrDefault()!.Id,
                Price = product.Prices!.FirstOrDefault()
            };

        await DbContext.WishLists.AddAsync(firstWishList, CancellationToken);
        await DbContext.WishLists.AddAsync(secondWishList, CancellationToken);
        await DbContext.SaveChangesAsync();

        //Act
        var wishListViewModels = await _wishListRepository.GetByIdWithInclude(
            user.Id,
            CancellationToken
        );

        //Assert
        Assert.Equal(2, wishListViewModels.Count);
    }
}
