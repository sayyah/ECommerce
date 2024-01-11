using ECommerce.API.DataTransferObject.Keywords;
using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Keywords;

public class EditModel(IKeywordService keywordService) : PageModel
{
    [BindProperty] public ReadKeywordDto? Keyword { get; set; }
    [TempData] public string? Message { get; set; }
    [TempData] public string? Code { get; set; }

    public async Task OnGet(int id)
    {
        var result = await keywordService.GetById(id);
        Keyword = result.ReturnData;
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            if (Keyword != null)
            {
                var result = await keywordService.Edit(Keyword);
                Message = result.Message;
                Code = result.Code.ToString();
                if (result.Code == 0)
                    return RedirectToPage("/Keywords/Index",
                        new { area = "Admin", message = result.Message, code = result.Code.ToString() });
                Message = result.Message;
                Code = result.Code.ToString();
                if (result.Message != null) ModelState.AddModelError("", result.Message);
            }
        }

        return Page();
    }
}