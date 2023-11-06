using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IUserRepository : IAsyncRepository<User>
{
    Task<PagedList<UserListViewModel>> Search(UserFilterdParameters paginationParameters,
        CancellationToken cancellationToken);

    Task<bool> Exists(int id, string email, string phoneNumber, CancellationToken cancellationToken);
    Task<User?> GetByEmailOrUserName(string input, CancellationToken cancellationToken);
    Task<User> GetByPhoneNumber(string phone, CancellationToken cancellationToken);
    Task<List<UserRole>> GetUserRoles(int id, CancellationToken cancellationToken);
    Task<List<UserRole>> GetApplicationRoles(CancellationToken cancellationToken);
    Task AddLoginHistory(int userId, string token, string ipAddress, DateTime expirationDate);

    Task<bool> SetConfirmCodeByUsername(string username, int confirmCode, DateTime codeConfirmExpairDate,
        CancellationToken cancellationToken);

    Task<int?> GetSecondsLeftConfirmCodeExpire(string username, CancellationToken cancellationToken);
}
