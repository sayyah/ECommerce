using System.Diagnostics;

namespace ECommerce.Front.BolouriGroup.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel(ILogger<ErrorModel> logger) : PageModel
{
    private readonly ILogger<ErrorModel> _logger = logger;

    public string RequestId { get; set; }
    public string Message { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public void OnGet(string message = "")
    {
        Message = message;
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}