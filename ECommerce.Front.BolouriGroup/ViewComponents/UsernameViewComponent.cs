using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.ViewComponents;

public class UsernameViewComponent(ICookieService cookieService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var result = cookieService.GetCurrentUser();
        return View(result);
    }
}