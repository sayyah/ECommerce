using ECommerce.Application.ViewModels;

namespace ECommerce.Services.IServices;

public interface IWishListService
{
    Task<ServiceResult<List<WishListViewModel>>> Load();
    Task<ServiceResult> Add(int productId);
    Task<ServiceResult> Delete(int wishListId);
    Task<ServiceResult> Invert(int priceId);
}