using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IWishListRepository : IRepositoryBase<WishList>
{
    Task<WishList?> GetByProductUser(int productId, int userId, CancellationToken cancellationToken);
    Task<List<WishListViewModel>?> GetByIdWithInclude(int userId, CancellationToken cancellationToken);
}
