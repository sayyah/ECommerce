using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Brands;

public class BrandAddAsyncTests : BrandBaseTests
{
    [Fact]
    public async void AddAsync_AddEntity_ReturnsAddedEntity()
    {
        // Arrange
        var name = Guid.NewGuid().ToString();
        Brand brand = new()
        {
            Name = name,
            Description = Guid.NewGuid().ToString(),
            Url = Guid.NewGuid().ToString()
        };

        // Act
        var result = await BrandRepository.AddAsync(brand, CancellationToken);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        Assert.Equal(name, result.Name);
        Assert.Single(DbContext.Brands);
    }

    [Fact]
    public async void AddAsync_NoSave_EmptyEntities()
    {
        // Arrange
        var name = Guid.NewGuid().ToString();
        Brand brand = new()
        {
            Name = name,
            Description = Guid.NewGuid().ToString(),
            Url = Guid.NewGuid().ToString()
        };

        // Act
        await BrandRepository.AddAsync(brand, CancellationToken);

        // Assert
        Assert.Empty(DbContext.Brands);
    }

    [Fact]
    public async Task AddAsync_NullValue_ThrowsException()
    {
        // Act
        async Task Action() => await BrandRepository.AddAsync(null!, CancellationToken);
       

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
        
    }
}
