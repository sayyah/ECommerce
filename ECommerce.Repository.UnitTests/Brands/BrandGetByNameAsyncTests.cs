using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Brands;

public class BrandGetByNameAsyncTests : BrandBaseTests
{
    [Fact]
    public async void GetByNameAsync_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        var name = Guid.NewGuid().ToString();
        Brand brand = new()
        {
            Id = 2,
            Name = name,
            Url = Guid.NewGuid().ToString()
        };
        DbContext.Brands.Add(brand);
        await DbContext.SaveChangesAsync();

        // Act        
        var result = await BrandRepository.GetByName(name, CancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Name);
    }

    [Fact]
    public async void GetByNameAsync_GetEntityByNotExistName_ReturnNull()
    {
        // Arrange
        const string name = "no name";

        // Act
        var result = await BrandRepository.GetByName(name, CancellationToken);

        // Assert
        Assert.Null(result);
    }
}
