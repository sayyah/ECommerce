using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.States;

public class DeleteModel(IStateService stateService) : PageModel
{
    public State State { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

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

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await stateService.Delete(id);
            return RedirectToPage("/States/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}