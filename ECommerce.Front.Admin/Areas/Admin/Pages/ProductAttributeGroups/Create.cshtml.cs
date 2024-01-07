using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.ProductAttributeGroups;

public class CreateModel(IProductAttributeGroupService productAttributeGroupService) : PageModel
{
    [BindProperty] public ProductAttributeGroup ProductAttributeGroup { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var result = await productAttributeGroupService.Add(ProductAttributeGroup);
            if (result.Code == 0)
                return RedirectToPage("/ProductAttributeGroups/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }
}