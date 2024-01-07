using Microsoft.Extensions.Logging;

namespace ECommerce.Front.Admin.Areas.Admin.Pages;

//[Authorize(AuthenticationSchemes ="ClientCookie")]
public class IndexModel(ILogger<IndexModel> logger) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;

    public void OnGet()
    {
    }
}