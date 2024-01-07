using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Pages;

public class WishlistModel(IWishListService wishListService) : PageModel
{
    public ServiceResult<List<WishListViewModel>> WishList { get; set; }

    public async Task OnGet()
    {
        WishList = await wishListService.Load();
    }
}