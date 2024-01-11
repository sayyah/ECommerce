using ECommerce.API.DataTransferObject.Keywords;
using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Keywords;

public class DetailModel(IKeywordService keywordService) : PageModel
{
    public ReadKeywordDto? Keyword { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await keywordService.GetById(id);
        if (result.Code == 0)
        {
            Keyword = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Keywords/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}