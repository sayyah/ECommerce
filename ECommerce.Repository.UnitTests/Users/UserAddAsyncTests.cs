using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Users;

public class UserAddAsyncTests : UserBaseTests
{
    [Fact]
    public async void AddAsync_AddEntity_ReturnsAddedEntity()
    {
        // Arrange
        const int expectedCount = 2;
        var name = Guid.NewGuid().ToString();
        var mobile ="09119876543";
        User user = new()
        {
            UserName = name,
            Mobile = mobile
        };

        // Act
        var result = await UserRepository.AddAsync(user, CancellationToken);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        Assert.Equal(name, result.UserName);
        Assert.Equal(mobile, result.Mobile);
        Assert.Equal(expectedCount, DbContext.Users.Count());
    }

    [Fact]
    public async void AddAsync_NoSave_EmptyEntities()
    {
        // Arrange
        const int defaultUsersCount = 1;
        var name = Guid.NewGuid().ToString();
        User user = new()
        {
            UserName = name
        };

        // Act
        await UserRepository.AddAsync(user, CancellationToken);

        // Assert
        Assert.Equal(defaultUsersCount, DbContext.Users.Count());
    }

    [Fact]
    public async Task AddAsync_NullValue_ThrowsException()
    {
        // Act
        async Task Action() => await UserRepository.AddAsync(null!, CancellationToken);


        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }
}
