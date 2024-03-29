using ECommerce.Entities.ViewModel;
using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerce.Front.ArshaHamrah.Areas.Admin.Pages.Products;

[Authorize(Roles = "Admin,SuperAdmin")]
public class IndexModel : PageModel
{
    private readonly IProductService _productService;

    public IndexModel(IProductService productService)
    {
        _productService = productService;
    }

    //public PaginationViewModel PaginationViewModel { get; set; }
    public ServiceResult<List<ProductIndexPageViewModel>> Products { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task OnGet(string search = "", int pageNumber = 1, int pageSize = 10, string message = null,
        string code = null)
    {
        Message = message;
        Code = code;
        var result = await _productService.Search(search, pageNumber, pageSize);
        if (result.Code == ServiceCode.Success)
        {
            Message = result.Message;
            Code = result.Code.ToString();
            Products = result;
        }
    }
}