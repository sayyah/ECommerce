using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Units;

public class DetailModel(IUnitService unitService) : PageModel
{
    public Unit Unit { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await unitService.GetById(id);
        if (result.Code == 0)
        {
            Unit = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Units/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}