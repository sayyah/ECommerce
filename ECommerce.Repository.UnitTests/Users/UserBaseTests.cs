using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;

namespace ECommerce.Repository.UnitTests.Users;

public class UserBaseTests : BaseTests
{
    public readonly IUserRepository UserRepository;

    public UserBaseTests()
    {
        UserRepository = new UserRepository(DbContext);
    }
}
