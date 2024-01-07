using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Products;

public class AttributeValueModel(IProductAttributeGroupService attributeGroupService) : PageModel
{
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }

    public List<ProductAttributeGroup> AttributeGroups { get; set; }
    [BindProperty] public int ProductId { get; set; }

    public async Task OnGet(int id, string search = "", int pageIndex = 1, int quantityPerPage = 10,
        string message = null, string code = null)
    {
        Message = message;
        Code = code;
        ProductId = id;
        var result = await attributeGroupService.GetByProductId(id);
        if (result.Code == ServiceCode.Success) AttributeGroups = result.ReturnData;
    }

    public async Task<IActionResult> OnPost(List<ProductAttributeGroup> attributeGroups)
    {
        var result = await attributeGroupService.AddWithAttributeValue(attributeGroups, ProductId);
        return RedirectToAction("/Products/AttributeValue", new { id = ProductId });
    }
}