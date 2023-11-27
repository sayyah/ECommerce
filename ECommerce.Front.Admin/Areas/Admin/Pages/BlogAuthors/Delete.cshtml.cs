using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Areas.Admin.Pages.BlogAuthors;

public class DeleteModel : PageModel
{
    private readonly IBlogAuthorService _blogAuthorService;

    public DeleteModel(IBlogAuthorService blogAuthorService)
    {
        _blogAuthorService = blogAuthorService;
    }

    public BlogAuthor BlogAuthor { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await _blogAuthorService.GetById(id);
        if (result.Code == 0)
        {
            BlogAuthor = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/BlogAuthors/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await _blogAuthorService.Delete(id);
            return RedirectToPage("/BlogAuthors/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}