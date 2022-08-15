using Entities.Helper;
using Entities.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ECommerce.Services.IServices;

namespace ArshaHamrah.Pages;

public class WishListModel : PageModel
{
    private readonly IWishListService _wishListService;

    public WishListModel(IWishListService wishListService)
    {
        _wishListService = wishListService;
    }

    public ServiceResult<List<WishListViewModel>> WishList { get; set; }

    public async Task OnGet()
    {
        WishList = await _wishListService.Load();
    }
}