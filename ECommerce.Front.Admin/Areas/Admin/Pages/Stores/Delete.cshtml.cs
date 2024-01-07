using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Stores;

public class DeleteModel(IStoreService storeService) : PageModel
{
    public Store Store { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await storeService.GetById(id);
        if (result.Code == 0)
        {
            Store = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Stores/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await storeService.Delete(id);
            return RedirectToPage("/Stores/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}