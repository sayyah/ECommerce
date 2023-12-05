using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Brands;

public class BrandGetByIdAsyncTests : BrandBaseTests
{
    [Fact]
    public async void GetByIdAsync_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        const int id = 1;
        Brand brand = new()
        {
            Id = id,
            Name = Guid.NewGuid().ToString(),
            Url = Guid.NewGuid().ToString()
        };
        DbContext.Brands.Add(brand);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await BrandRepository.GetByIdAsync(CancellationToken, id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void GetByIdAsync_GetEntityByFalseId_ReturnNull()
    {
        // Arrange
        const int id = 0;

        // Act
        var result = await BrandRepository.GetByIdAsync(CancellationToken, id);

        // Assert
        Assert.Null(result);
    }
}
