using ECommerce.API.DataTransferObject.Tags;
using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Tags;

public class DetailModel(ITagService tagService) : PageModel
{
    public ReadTagDto? Tag { get; set; }

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
}