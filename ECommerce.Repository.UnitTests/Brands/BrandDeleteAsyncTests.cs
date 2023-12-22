using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Brands;

public class BrandDeleteAsyncTests : BrandBaseTests
{
    [Fact]
    public async void DeleteAsync_DeleteExistEntity_EntityIsInRepository()
    {
        // Arrange
        Brand brand = new()
        {
            Id = 2,
            Name = Guid.NewGuid().ToString(),
            Url = Guid.NewGuid().ToString()
        };
        DbContext.Brands.Add(brand);
        await DbContext.SaveChangesAsync();

        // Act        
        BrandRepository.Delete(brand);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        Assert.Empty(DbContext.Brands);
    }

    [Fact]
    public void DeleteAsync_NullBrand_ThrowsException()
    {
        // Act        
        void Action() => BrandRepository.Delete(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);

    }
}
