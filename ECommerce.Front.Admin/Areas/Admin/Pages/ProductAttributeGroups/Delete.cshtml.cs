using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.ProductAttributeGroups;

public class DeleteModel(IProductAttributeGroupService productAttributeGroupService) : PageModel
{
    public ProductAttributeGroup ProductAttributeGroup { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

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

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await productAttributeGroupService.Delete(id);
            return RedirectToPage("/ProductAttributeGroups/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}