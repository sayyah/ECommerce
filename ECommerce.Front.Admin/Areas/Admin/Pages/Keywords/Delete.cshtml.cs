using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Keywords;

public class DeleteModel(IKeywordService keywordService) : PageModel
{
    public Keyword Keyword { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

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

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await keywordService.Delete(id);
            return RedirectToPage("/Keywords/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}