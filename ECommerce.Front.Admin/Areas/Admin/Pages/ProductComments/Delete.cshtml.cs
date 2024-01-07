using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.ProductComments;

public class DeleteModel(IProductCommentService productComment, IProductService productService)
    : PageModel
{
    [BindProperty] public ProductComment ProductComment { get; set; }
    public Product Product { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }

    public async Task OnGet(int id)
    {
        var ProductCommentResult = await productComment.GetById(id);
        ProductComment = ProductCommentResult.ReturnData;
        var _productId = ProductComment.ProductId ?? default(int);
        var ProductResult = await productService.GetById(_productId);
        Product = ProductResult.ReturnData;
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            if (ProductComment.AnswerId != null) await productComment.Delete(ProductComment.AnswerId ?? default(int));
            var result = await productComment.Delete(ProductComment.Id);
            return RedirectToPage("/ProductComments/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}

