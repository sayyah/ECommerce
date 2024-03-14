using ECommerce.API.DataTransferObject.BlogComments.Queries;
using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.BlogComments;

public class DeleteModel(IBlogCommentService blogCommentService, IBlogService blogService)
    : PageModel
{
    [BindProperty] public BlogComment BlogComment { get; set; }
    public ReadBlogDto Blog { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }

    public async Task OnGet(int id)
    {
        var BlogCommentResult = await blogCommentService.GetById(id);
        BlogComment = BlogCommentResult.ReturnData;
        var _blogId = BlogComment.BlogId ?? default(int);
        var ProductResult = await blogService.GetById(_blogId);
        Blog = ProductResult.ReturnData;
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            if (BlogComment.AnswerId != null) await blogCommentService.Delete(BlogComment.AnswerId ?? default(int));
            var result = await blogCommentService.Delete(BlogComment.Id);
            return RedirectToPage("/BlogComments/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}

