using ECommerce.Services.IServices;
<<<<<<< HEAD:ECommerce.Front.Admin/Areas/Admin/Pages/ProductComments/Index.cshtml.cs

namespace ECommerce.Front.Admin.Areas.Admin.Pages.ProductComments;

=======

namespace ECommerce.Front.BolouriGroup.Areas.Admin.Pages.ProductComments;

>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8):ECommerce.Front.BolouriGroup/Areas/Admin/Pages/ProductComments/Index.cshtml.cs
public class IndexModel : PageModel
{
    private readonly IProductCommentService _productComments;

    public IndexModel(IProductCommentService productCommentService)
    {
        _productComments = productCommentService;
    }

    public ServiceResult<List<ProductComment>> ProductComments { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(string search = "", int pageNumber = 1, int pageSize = 10,
        string message = null, string code = null)
    {
        Message = message;
        Code = code;
        var result = await _productComments.Load(search, pageNumber, pageSize);
        if (result.Code == ServiceCode.Success)
        {
            if (Message != null)
            {
                Message = Message;
                Code = Code;
            }
            else
            {
                Message = result.Message;
                Code = result.Code.ToString();
            }

            ProductComments = result;
            return Page();
        }

        return RedirectToPage("/index", new { message = result.Message, code = result.Code.ToString() });
    }
}
 
