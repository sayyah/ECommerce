using Ecommerce.Entities;
using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerce.Front.BolouriGroup.Areas.Admin.Pages.Keywords;

public class DeleteModel : PageModel
{
    private readonly IKeywordService _keywordService;

    public DeleteModel(IKeywordService keywordService)
    {
        _keywordService = keywordService;
    }

    public Keyword Keyword { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await _keywordService.GetById(id);
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
            var result = await _keywordService.Delete(id);
            return RedirectToPage("/Keywords/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}