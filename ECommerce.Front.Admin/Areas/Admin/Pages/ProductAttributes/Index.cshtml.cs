using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Front.BolouriGroup.Areas.Admin.Pages.ProductAttributes;

public class IndexModel : PageModel
{
    private readonly IProductAttributeGroupService _productAttributeGroupService;
    private readonly IProductAttributeService _productAttributeService;

    public IndexModel(IProductAttributeService productAttributeService,
        IProductAttributeGroupService productAttributeGroupService)
    {
        _productAttributeService = productAttributeService;
        _productAttributeGroupService = productAttributeGroupService;
    }

    public List<ProductAttribute> ProductAttributes { get; set; }
    public SelectList AttributeGroup { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task OnGet(int attributeGroupId = 0, string message = null, string code = null)
    {
        Message = message;
        Code = code;
        var attributeGroups = (await _productAttributeGroupService.GetAll()).ReturnData;
        AttributeGroup = new SelectList(attributeGroups, nameof(ProductAttributeGroup.Id),
            nameof(ProductAttributeGroup.Name));
        if (attributeGroupId == 0)
            attributeGroupId = attributeGroups.FirstOrDefault().Id;
        var result = await _productAttributeService.GetAllAttributeWithGroupId(attributeGroupId, 10);
        ProductAttributes = result.ReturnData;
    }
}