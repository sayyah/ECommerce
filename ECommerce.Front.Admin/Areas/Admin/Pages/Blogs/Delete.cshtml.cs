using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Blogs;

public class DeleteModel(IBlogService blogService) : PageModel
{
    public Blog Blog { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

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

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await blogService.Delete(id);
            return RedirectToPage("/Blogs/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}