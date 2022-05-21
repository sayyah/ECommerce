using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ECommerce.Services.IServices;

namespace ArshaHamrah.Areas.Admin.Pages.ProductAttributeGroups;

public class IndexModel : PageModel
{
    private readonly IProductAttributeGroupService _productAttributeGroupService;

    public IndexModel(IProductAttributeGroupService productAttributeGroupService)
    {
        _productAttributeGroupService = productAttributeGroupService;
    }

    public List<ProductAttributeGroup> ProductAttributeGroups { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task OnGet(string message = null, string code = null)
    {
        Message = message;
        Code = code;
        var result = await _productAttributeGroupService.Load();
        ProductAttributeGroups = result.ReturnData;
    }
}