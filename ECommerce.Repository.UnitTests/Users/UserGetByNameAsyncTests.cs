using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Users;

public class UserGetByNameAsyncTests : UserBaseTests
{
    [Fact]
    public async void GetByNameAsync_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        var userName = Guid.NewGuid().ToString();
        User user = new()
        {
            Id = 2,
            UserName = userName,
            Mobile = "2345678900"
        };
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        // Act        
        var result = await UserRepository.GetByEmailOrUserName(userName, CancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userName, result.UserName);
    }

    [Fact]
    public async void GetByNameAsync_GetEntityByNotExistName_ReturnNull()
    {
        // Arrange
        const string name = "no name";

        // Act
        var result = await UserRepository.GetByEmailOrUserName(name, CancellationToken);

        // Assert
        Assert.Null(result);
    }
}
