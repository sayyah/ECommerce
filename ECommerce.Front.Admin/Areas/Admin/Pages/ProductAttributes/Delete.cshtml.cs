using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.ProductAttributes;

public class DeleteModel(IProductAttributeService productAttributeService) : PageModel
{
    public ProductAttribute ProductAttribute { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await productAttributeService.GetById(id);
        if (result.Code == 0)
        {
            ProductAttribute = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/ProductAttributes/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await productAttributeService.Delete(id);
            return RedirectToPage("/ProductAttributes/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}