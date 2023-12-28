using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;
using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(SunflowerECommerceDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PagedList<UserListViewModel>> Search(userFilterParameters userFilteredParameters,
        CancellationToken cancellationToken)
    {
        var query = TableNoTracking.Where(x => x.UserName.Contains(userFilteredParameters.PaginationParameters.Search))
            .AsNoTracking();

        if (userFilteredParameters.IsActive != null)
            query = query.Where(x => x.IsActive == userFilteredParameters.IsActive);
        if (userFilteredParameters.IsColleauge != null)
            query = query.Where(x => x.IsColleague == userFilteredParameters.IsColleauge);
        if (userFilteredParameters.HasBuying != null)
            query = userFilteredParameters.HasBuying == true
                ? query.Where(x => x.PurchaseOrders.Count > 0)
                : query.Where(x => x.PurchaseOrders.Count == 0);

        var sortedQuery = query.OrderByDescending(x => x.Id).ToList();

        switch (userFilteredParameters.UserSort)
        {
            case UserSort.LowToHighCountBuying:
                sortedQuery = query.OrderBy(x => x.PurchaseOrders.Count).ToList();
                break;
            case UserSort.HighToLowCountBuying:
                sortedQuery = query.OrderByDescending(x => x.PurchaseOrders.Count).ToList();
                break;
            case UserSort.LowToHighPiceBuying:
                sortedQuery = query.OrderBy(x => x.PurchaseOrders.Sum(p => p.Amount)).ToList();
                break;
            case UserSort.HighToLowPriceBuying:
                sortedQuery = query.OrderByDescending(x => x.PurchaseOrders.Sum(p => p.Amount)).ToList();
                break;
        }

        var userList = await query.Select(u => new UserListViewModel
        {
            Id = u.Id,
            BuyingAmount = u.PurchaseOrders.Sum(s => s.Amount),
            City = u.City.Name,
            State = u.State.Name,
            IsActive = u.IsActive,
            IsColleague = u.IsColleague,
            RegisterDate = u.RegisterDate,
            Username = u.UserName,
            UserRole = u.UserRole.Name
        }).ToListAsync(cancellationToken);
        return PagedList<UserListViewModel>.ToPagedList(userList,
            userFilteredParameters.PaginationParameters.PageNumber,
            userFilteredParameters.PaginationParameters.PageSize);
    }

    public async Task<User?> GetByEmailOrUserName(string input, CancellationToken cancellationToken)
    {
        return await TableNoTracking.Where(p => p.Email == input || p.PhoneNumber == input || p.UserName == input)
            .Include(x => x.UserRole).AsNoTracking().SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<User> GetByPhoneNumber(string phone, CancellationToken cancellationToken)
    {
        return await TableNoTracking.Where(p => p.PhoneNumber == phone).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> Exists(int id, string email, string phoneNumber, CancellationToken cancellationToken)
    {
        return await TableNoTracking.AnyAsync(p => p.Id != id && (p.Email == email || p.PhoneNumber == phoneNumber),
            cancellationToken);
    }

    public async Task<List<UserRole>> GetUserRoles(int id, CancellationToken cancellationToken)
    {
        var userRoles = await DbContext.UserRoles.AsNoTracking().Where(q => q.UserId == id).Select(p => p.RoleId)
            .ToListAsync(cancellationToken);
        return await DbContext.Roles.Where(p => userRoles.Contains(p.Id)).ToListAsync(cancellationToken);
    }

    public async Task<List<UserRole>> GetApplicationRoles(CancellationToken cancellationToken)
    {
        return await DbContext.Roles.ToListAsync(cancellationToken);
    }

    public void AddLoginHistory(int userId, string token, string ipAddress, DateTime expirationDate)
    {
        DbContext.LoginHistories.Add(new LoginHistory
        {
            CreationDate = DateTime.Now,
            ExpirationDate = expirationDate,
            IpAddress = ipAddress.Trim(),
            Token = token.Trim(),
            UserId = userId
        });
    }

    public void SetConfirmCodeByUsername(string username, int confirmCode, DateTime codeConfirmExpireDate)
    {
        var user = TableNoTracking.FirstOrDefault(x => x.UserName == username);
        if (user == null) return;
        user.ConfirmCode = confirmCode;
        user.ConfirmCodeExpirationDate = codeConfirmExpireDate;
        DbContext.Update(user);
    }

    public async Task<int?> GetSecondsLeftConfirmCodeExpire(string username, CancellationToken cancellationToken)
    {
        var user = await TableNoTracking.Where(x => x.UserName == username).FirstOrDefaultAsync(cancellationToken);
        if (user == null) return null;
        if (user.ConfirmCodeExpirationDate == null) return 0;
        var result = (int)(user.ConfirmCodeExpirationDate - DateTime.Now).Value.TotalSeconds;
        if (result < 1) return 0;
        return result;
    }
}
