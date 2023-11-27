using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Areas.Admin.Pages.BlogComments;

public class DeleteModel : PageModel
{
    private readonly IBlogCommentService _blogCommentService;

    private readonly IBlogService _blogService;

    public DeleteModel(IBlogCommentService blogCommentService, IBlogService blogService)
    {
        _blogCommentService = blogCommentService;
        _blogService = blogService;
    }

    [BindProperty] public BlogComment BlogComment { get; set; }
    public Blog Blog { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }

    public async Task OnGet(int id)
    {
        var BlogCommentResult = await _blogCommentService.GetById(id);
        BlogComment = BlogCommentResult.ReturnData;
        var _blogId = BlogComment.BlogId ?? default(int);
        var ProductResult = await _blogService.GetById(_blogId);
        Blog = ProductResult.ReturnData;
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            if (BlogComment.AnswerId != null) await _blogCommentService.Delete(BlogComment.AnswerId ?? default(int));
            var result = await _blogCommentService.Delete(BlogComment.Id);
            return RedirectToPage("/BlogComments/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}
