using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.ViewComponents;

public class UsernameViewComponent : ViewComponent
{
    private readonly ICookieService _cookieService;

    public UsernameViewComponent(ICookieService cookieService)
    {
        _cookieService = cookieService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var result = _cookieService.GetCurrentUser();
        return View(result);
    }
}