using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Brands;

public class BrandUpdateAsyncTests : BrandBaseTests
{
    [Fact]
    public async void UpdateAsync_UpdateEntity_EntityChanges()
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
        var newName = Guid.NewGuid().ToString();
        brand.Name = newName;
        BrandRepository.Update(brand);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        Assert.Equal(newName, DbContext.Brands.FirstOrDefault(x => x.Id == 2)!.Name);
    }

    [Fact]
    public void UpdateAsync_UpdateNull_ThrowsException()
    {
        // Act        
        void Action() => BrandRepository.Update(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);

    }
}
