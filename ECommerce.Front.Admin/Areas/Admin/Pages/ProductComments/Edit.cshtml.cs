using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.ProductComments;

public class EditModel(IProductCommentService productCommentService, IProductService productService)
    : PageModel
{
    private readonly IProductService _productService = productService;

    [BindProperty] public ProductComment ProductComment { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }

    public async Task OnGet(int id, string message = null, string code = null)
    {
        Message = message;
        Code = code;
        var ProductCommentResult = await productCommentService.GetById(id);
        ProductComment = ProductCommentResult.ReturnData;
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            if (ProductComment.Answer!.Text == null && ProductComment.AnswerId == null) ProductComment.Answer = null;
            var result = await productCommentService.Edit(ProductComment);
            Message = result.Message;
            Code = result.Code.ToString();
            if (result.Code == 0)
                return RedirectToPage("/ProductComments/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
            return RedirectToPage("/ProductComments/Edit",
                new { id = ProductComment.Id, area = "Admin", message = $"پیغام خطا:{Message}", code = Code });
        }
        catch (Exception ex)
        {
            return RedirectToPage("/ProductComments/Edit",
                new { id = ProductComment.Id, area = "Admin", message = "پیغام خطای غیر منتظره", code = "Error" });
        }
    }
}

