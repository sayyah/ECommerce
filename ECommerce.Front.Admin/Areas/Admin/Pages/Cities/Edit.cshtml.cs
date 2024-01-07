using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Cities;

public class EditModel(ICityService cityService, IStateService stateService) : PageModel
{
    [BindProperty] public City City { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }
    public SelectList StateCity { get; set; }

    public async Task OnGet(int id)
    {
        var result = await cityService.GetById(id);
        City = result.ReturnData;

        var stateCity = (await stateService.GetAll()).ReturnData;
        StateCity = new SelectList(stateCity, nameof(State.Id),
            nameof(State.Name));
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var result = await cityService.Edit(City);
            Message = result.Message;
            Code = result.Code.ToString();
            if (result.Code == 0)
                return RedirectToPage("/Cities/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }
}