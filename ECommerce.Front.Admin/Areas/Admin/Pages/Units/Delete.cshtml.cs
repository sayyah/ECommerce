using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Areas.Admin.Pages.Units;

public class DeleteModel : PageModel
{
    private readonly IUnitService _unitService;

    public DeleteModel(IUnitService unitService)
    {
        _unitService = unitService;
    }

    public Unit Unit { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await _unitService.GetById(id);
        if (result.Code == 0)
        {
            Unit = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Units/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await _unitService.Delete(id);
            return RedirectToPage("/Units/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}