using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Products;

public class DetailModel(IProductService productService) : PageModel
{
    [BindProperty] public Product Product { get; set; }
    [BindProperty] public IFormFile Upload { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await productService.GetById(id);
        if (result.Code == 0)
        {
            Product = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Products/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}