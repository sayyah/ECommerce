using ECommerce.Services.IServices;
<<<<<<< HEAD:ECommerce.Front.Admin/Areas/Admin/Pages/Products/Detail.cshtml.cs
using Microsoft.AspNetCore.Http;
=======
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8):ECommerce.Front.BolouriGroup/Areas/Admin/Pages/Products/Detail.cshtml.cs

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Products;

public class DetailModel : PageModel
{
    private readonly IProductService _productService;


    public DetailModel(IProductService productService)
    {
        _productService = productService;
    }

    [BindProperty] public Product Product { get; set; }
    [BindProperty] public IFormFile Upload { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await _productService.GetById(id);
        if (result.Code == 0)
        {
            Product = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Products/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}