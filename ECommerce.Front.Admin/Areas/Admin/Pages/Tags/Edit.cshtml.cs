using ECommerce.API.DataTransferObject.Tags;
using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Tags;

public class EditModel(ITagService tagService) : PageModel
{
    [BindProperty] public ReadTagDto? Tag { get; set; }
    [TempData] public string? Message { get; set; }
    [TempData] public string? Code { get; set; }

    public async Task OnGet(int id)
    {
        var result = await tagService.GetById(id);
        Tag = result.ReturnData;
    }

    public async Task<IActionResult> OnPost()
    {
        if (Tag.TagText.Contains("@") || Tag.TagText.Contains("&") || Tag.TagText.Contains("*") ||
            Tag.TagText.Contains("/") || Tag.TagText.Contains("\\"))
        {
            Message = "از علامت های @ & * / \\ استفاده نکنید";
            Code = "Error";
            return Page();
        }

        if (ModelState.IsValid)
        {
            var result = await tagService.Edit(Tag);
            Message = result.Message;
            Code = result.Code.ToString();
            if (result.Code == 0)
                return RedirectToPage("/Tags/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            if (result.Message != null) ModelState.AddModelError("", result.Message);
        }

        return Page();
    }
}