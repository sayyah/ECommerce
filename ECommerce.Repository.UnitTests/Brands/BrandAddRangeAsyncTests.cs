using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Brands;

public class BrandAddRangeAsyncTests : BrandBaseTests
{
    [Fact]
    public async void AddRangeAsync_AddEntities_EntityExistsInRepository()
    {
        const int expectedCount = 2;
        // Arrange
        List<Brand> brands = new()
        {
             new Brand() { Name = Guid.NewGuid().ToString(), Url = Guid.NewGuid().ToString() },
             new Brand() { Name = Guid.NewGuid().ToString(), Url = Guid.NewGuid().ToString() }
        };
       
        // Act
        BrandRepository.AddRange(brands);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        Assert.Equal(expectedCount, DbContext.Brands.Count());
    }

    [Fact]
    public void AddRangeAsync_NullBlogCategory_ThrowsException()
    {
        // Act
        void Action() => BrandRepository.AddRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
      
    }
}
