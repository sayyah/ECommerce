using ECommerce.API.DataTransferObject.Keywords;
using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Keywords;

public class CreateModel(IKeywordService keywordService) : PageModel
{
    [BindProperty] public ReadKeywordDto? Keyword { get; set; }

    [TempData] public string? Message { get; set; }

    [TempData] public string? Code { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var result = await keywordService.Add(Keyword);
            if (result.Code == 0)
                return RedirectToPage("/Keywords/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }
}