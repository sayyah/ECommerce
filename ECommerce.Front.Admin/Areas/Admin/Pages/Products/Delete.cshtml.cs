using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Products;

public class DeleteModel(IProductService productService) : PageModel
{
    [BindProperty] public Product Product { get; set; }
    [BindProperty] public IFormFile Upload { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

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

    public async Task<IActionResult> OnPost(int id)
    {
        var result = await productService.Delete(id);
        return RedirectToPage("/Products/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}