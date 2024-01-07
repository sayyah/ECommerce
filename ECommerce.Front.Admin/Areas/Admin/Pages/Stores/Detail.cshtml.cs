using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Stores;

public class DetailModel(IStoreService storeService) : PageModel
{
    public Store Store { get; set; }

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
}