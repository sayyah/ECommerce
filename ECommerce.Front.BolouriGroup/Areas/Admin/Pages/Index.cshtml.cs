﻿namespace ECommerce.Front.BolouriGroup.Areas.Admin.Pages;

//[Authorize(AuthenticationSchemes ="ClientCookie")]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}