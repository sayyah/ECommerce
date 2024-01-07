using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.ProductAttributes;

public class DetailModel(IProductAttributeService productAttributeService) : PageModel
{
    public ProductAttribute ProductAttribute { get; set; }

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
}