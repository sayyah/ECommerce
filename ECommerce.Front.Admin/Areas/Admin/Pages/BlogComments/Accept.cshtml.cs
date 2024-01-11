using ECommerce.API.DataTransferObject.Blogs.Queries;
using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.BlogComments;

public class AcceptModel(IBlogCommentService blogCommentService, IBlogService blogService)
    : PageModel
{
    [BindProperty] public BlogComment BlogComment { get; set; }
    [TempData] public string? Message { get; set; }
    [TempData] public string Code { get; set; }
    public ReadBlogDto Blog { get; set; }

    public async Task OnGet(int id, string message = null, string code = null)
    {
        Message = message;
        Code = code;
        var BlogCommentResult = await blogCommentService.GetById(id);
        BlogComment = BlogCommentResult.ReturnData;
        var _blogId = BlogComment.BlogId ?? default(int);
        var ProductResult = await blogService.GetById(_blogId);
        Blog = ProductResult.ReturnData;
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            if (BlogComment.Answer!.Text == null && BlogComment.AnswerId == null) BlogComment.Answer = null;
            var result = await blogCommentService.Accept(BlogComment);
            Message = result.Message;
            Code = result.Code.ToString();
            if (result.Code == 0)
                return RedirectToPage("/BlogComments/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
            return RedirectToPage("/BlogComments/Accept",
                new { id = BlogComment.Id, area = "Admin", message = $"پیغام خطا:{Message}", code = Code });
        }
        catch
        {
            return RedirectToPage("/BlogComments/Accept",
                new { id = BlogComment.Id, area = "Admin", message = "پیغام خطای غیر منتظره", code = "Error" });
        }
    }
}