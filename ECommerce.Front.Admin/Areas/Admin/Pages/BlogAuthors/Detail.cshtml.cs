using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.BlogAuthors;

public class DetailModel(IBlogAuthorService blogAuthorService) : PageModel
{
    public BlogAuthor BlogAuthor { get; set; }

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
}