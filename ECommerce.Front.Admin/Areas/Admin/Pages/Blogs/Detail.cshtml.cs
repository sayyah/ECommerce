using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Blogs;

public class DetailModel(IBlogService blogService) : PageModel
{
    public Blog Blog { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await blogService.GetById(id);
        if (result.Code == 0)
        {
            Blog = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Blogs/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}