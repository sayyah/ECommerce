using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Cities;

public class CreateModel(ICityService cityService, IStateService stateService) : PageModel
{
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }
    public SelectList StateCity { get; set; }
    [BindProperty] public City City { get; set; }

    public async Task OnGet()
    {
        var stateCity = (await stateService.GetAll()).ReturnData;
        StateCity = new SelectList(stateCity, nameof(State.Id),
            nameof(State.Name));
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var result = await cityService.Add(City);
            if (result.Code == 0)
                return RedirectToPage("/Cities/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        var stateCity = (await stateService.GetAll()).ReturnData;
        StateCity = new SelectList(stateCity, nameof(State.Id),
            nameof(State.Name));
        return Page();
    }
}