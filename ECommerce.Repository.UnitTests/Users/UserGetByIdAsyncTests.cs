using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Users;

public class UserGetByIdAsyncTests : UserBaseTests
{
    [Fact]
    public async void GetByIdAsync_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        const int id = 2;
        User user = new()
        {
            Id = id,
            UserName = Guid.NewGuid().ToString(),
            Mobile = "12345678900"
        };
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await UserRepository.GetByIdAsync(CancellationToken, id);

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
        var result = await UserRepository.GetByIdAsync(CancellationToken, id);

        // Assert
        Assert.Null(result);
    }
}
