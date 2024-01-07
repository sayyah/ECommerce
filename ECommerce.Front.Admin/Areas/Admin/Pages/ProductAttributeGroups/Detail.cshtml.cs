using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.ProductAttributeGroups;

public class DetailModel(IProductAttributeGroupService productAttributeGroupService) : PageModel
{
    public ProductAttributeGroup ProductAttributeGroup { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await productAttributeGroupService.GetById(id);
        if (result.Code == 0)
        {
            ProductAttributeGroup = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/ProductAttributeGroups/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}