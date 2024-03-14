using ECommerce.API.DataTransferObject.BlogComments.Queries;
using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.BlogComments;

public class EditModel(IBlogCommentService blogCommentService, IBlogService blogService)
    : PageModel
{
    private readonly IBlogService _blogService = blogService;

    [BindProperty] public ReadBlogCommentDto BlogComment { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }

    public async Task OnGet(int id, string message = null, string code = null)
    {
        Message = message;
        Code = code;
        var blogCommentResult = await blogCommentService.GetById(id);
        BlogComment = blogCommentResult.ReturnData;
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            //if (BlogComment.AnswerText == null && BlogComment.AnswerId == null) BlogComment.Answer = null;
            var result = await blogCommentService.Edit(BlogComment);
            Message = result.Message;
            Code = result.Code.ToString();
            if (result.Code == 0)
                return RedirectToPage("/BlogComments/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
            return RedirectToPage("/BlogComments/Edit",
                new { id = BlogComment.Id, area = "Admin", message = $"پیغام خطا:{Message}", code = Code });
        }
        catch (Exception ex)
        {
            return RedirectToPage("/BlogComments/Edit",
                new { id = BlogComment.Id, area = "Admin", message = "پیغام خطای غیر منتظره", code = "Error" });
        }
    }
}
 
