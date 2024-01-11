using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Users;

public class UserDeleteAsyncTests : UserBaseTests
{
    [Fact]
    public async void DeleteAsync_DeleteExistEntity_EntityIsInRepository()
    {
        // Arrange
        User user = new()
        {
            UserName = Guid.NewGuid().ToString(),
            Mobile = "12345678900"
        };
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        // Act        
        UserRepository.Delete(user);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        Assert.Empty(DbContext.Brands);
    }

    [Fact]
    public void DeleteAsync_NullBrand_ThrowsException()
    {
        // Act        
        void Action() => UserRepository.Delete(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);

    }
}

