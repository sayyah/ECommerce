using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.BlogAuthors;

public class DeleteModel(IBlogAuthorService blogAuthorService) : PageModel
{
    public BlogAuthor BlogAuthor { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await blogAuthorService.GetById(id);
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
            var result = await blogAuthorService.Delete(id);
            return RedirectToPage("/BlogAuthors/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}