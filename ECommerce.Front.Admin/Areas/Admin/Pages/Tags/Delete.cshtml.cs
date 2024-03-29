using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Tags;

public class DeleteModel(ITagService tagService) : PageModel
{
    public Tag Tag { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await tagService.GetById(id);
        if (result.Code == 0)
        {
            Tag = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Tags/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await tagService.Delete(id);
            return RedirectToPage("/Tags/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}