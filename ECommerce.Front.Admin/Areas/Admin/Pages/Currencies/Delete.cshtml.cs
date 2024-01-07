using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Currencies;

public class DeleteModel(ICurrencyService currencyService) : PageModel
{
    public Currency Currency { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await currencyService.GetById(id);
        if (result.Code == 0)
        {
            Currency = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Currencies/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await currencyService.Delete(id);
            return RedirectToPage("/Currencies/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}