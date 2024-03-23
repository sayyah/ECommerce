using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IUserRepository : IRepositoryBase<User>
{
    PagedList<UserListViewModel> Search(userFilterParameters userFilteredParameters);

    Task<bool> Exists(int id, string email, string phoneNumber, CancellationToken cancellationToken);
    Task<User?> GetByEmailOrUserName(string input, CancellationToken cancellationToken);
    Task<User> GetByPhoneNumber(string phone, CancellationToken cancellationToken);
    Task<List<UserRole>> GetUserRoles(int id, CancellationToken cancellationToken);
    Task<List<UserRole>> GetApplicationRoles(CancellationToken cancellationToken);
    void AddLoginHistory(int userId, string token, string ipAddress, DateTime expirationDate);

   void SetConfirmCodeByUsername(string username, int confirmCode, DateTime codeConfirmExpireDate);

    Task<int?> GetSecondsLeftConfirmCodeExpire(string username, CancellationToken cancellationToken);
}
