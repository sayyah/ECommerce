using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Users;

public class UserAddRangeAsyncTests : UserBaseTests
{
    [Fact]
    public async void AddRangeAsync_AddEntities_EntityExistsInRepository()
    {
        const int expectedCount = 3;
        // Arrange
        List<User> users = new()
        {
            new User() { UserName = Guid.NewGuid().ToString(), Mobile = "12345678900" },
            new User() { UserName = Guid.NewGuid().ToString(), Mobile = "12345678900" }
        };

        // Act
        UserRepository.AddRange(users);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        Assert.Equal(expectedCount, DbContext.Users.Count());
    }

    [Fact]
    public void AddRangeAsync_NullBlogCategory_ThrowsException()
    {
        // Act
        void Action() => UserRepository.AddRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);

    }
}
