using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.States;

public class DetailModel(IStateService stateService) : PageModel
{
    public State State { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await stateService.GetById(id);
        if (result.Code == 0)
        {
            State = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/States/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}