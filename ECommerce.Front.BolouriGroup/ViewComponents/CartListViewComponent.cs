using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.ViewComponents;

public class CartListViewComponent(ICartService cartService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var cart = (await cartService.Load(HttpContext)).ReturnData;
        TempData["cartLength"] = cart.Count;
        return View(cart);
    }
}