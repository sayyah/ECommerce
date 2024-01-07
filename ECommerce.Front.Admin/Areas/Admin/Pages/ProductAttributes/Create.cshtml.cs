using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.ProductAttributes;

public class CreateModel(IProductAttributeService productAttributeService,
        IProductAttributeGroupService productAttributeGroupService)
    : PageModel
{
    [BindProperty] public ProductAttribute ProductAttribute { get; set; }
    public SelectList AttributeGroup { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task OnGet()
    {
        var attributeGroups = (await productAttributeGroupService.GetAll()).ReturnData;
        AttributeGroup = new SelectList(attributeGroups, nameof(ProductAttributeGroup.Id),
            nameof(ProductAttributeGroup.Name));
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var result = await productAttributeService.Add(ProductAttribute);
            if (result.Code == 0)
                return RedirectToPage("/ProductAttributes/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }
}