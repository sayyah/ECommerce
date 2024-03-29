using ECommerce.Entities;
using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerce.Front.ArshaHamrah.Areas.Admin.Pages.BlogAuthors;

[Authorize(Roles = "Admin,SuperAdmin")]
public class DetailModel : PageModel
{
    private readonly IBlogAuthorService _blogAuthorService;

    public DetailModel(IBlogAuthorService blogAuthorService)
    {
        _blogAuthorService = blogAuthorService;
    }

    public BlogAuthor BlogAuthor { get; set; }

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
}