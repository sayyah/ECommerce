using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.States;

public class CreateModel(IStateService stateService) : PageModel
{
    [BindProperty] public State State { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var result = await stateService.Add(State);
            if (result.Code == 0)
                return RedirectToPage("/States/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }
}