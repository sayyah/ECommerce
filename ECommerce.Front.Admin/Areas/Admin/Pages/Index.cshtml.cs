<<<<<<< HEAD:ECommerce.Front.Admin/Areas/Admin/Pages/Index.cshtml.cs
﻿using Microsoft.Extensions.Logging;

namespace ECommerce.Front.Admin.Areas.Admin.Pages;
=======
﻿namespace ECommerce.Front.BolouriGroup.Areas.Admin.Pages;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8):ECommerce.Front.BolouriGroup/Areas/Admin/Pages/Index.cshtml.cs

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