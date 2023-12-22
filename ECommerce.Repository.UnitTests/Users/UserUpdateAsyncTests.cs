using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Users;

public class UserUpdateAsyncTests : UserBaseTests
{
    [Fact]
    public async void UpdateAsync_UpdateEntity_EntityChanges()
    {
        // Arrange       
        User user = new()
        {
            Id = 2,
            UserName = Guid.NewGuid().ToString(),
            Mobile = "12345678900"
        };
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        // Act
        var newUserName = Guid.NewGuid().ToString();
        user.UserName = newUserName;
        UserRepository.Update(user);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        Assert.Equal(newUserName, DbContext.Users.FirstOrDefault(x => x.Id == 2)!.UserName);
    }

    [Fact]
    public void UpdateAsync_UpdateNull_ThrowsException()
    {
        // Act        
        void Action() => UserRepository.Update(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);

    }
}

