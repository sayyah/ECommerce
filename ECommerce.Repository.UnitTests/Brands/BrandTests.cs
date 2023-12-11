using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.Brands;

[CollectionDefinition("BrandTests", DisableParallelization = true)]
[Collection("BrandTests")]
public class BrandTests : BaseTests
{
    private readonly IBrandRepository _brandRepository;

    public BrandTests()
    {
        _brandRepository = new BrandRepository(DbContext);
    }

    [Fact]
    public async Task AddAsync_AddNewEntity_ReturnsNewEntity()
    {
        //Arrange
        var id = 1;
        var name = "Brand for test";
        var brand = new Brand
        {
            Id = id,
            Name = name
        };

        //Act
        var newBrand = await _brandRepository.AddAsync(brand, new CancellationToken());

        //Assert
        Assert.Equal(newBrand.Id, id);
        Assert.Equal(newBrand.Name, name);
    }
}