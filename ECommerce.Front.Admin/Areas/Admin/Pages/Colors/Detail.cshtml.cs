using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Colors;

public class DetailModel(IColorService colorService) : PageModel
{
    public Color Color { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await colorService.GetById(id);
        if (result.Code == 0)
        {
            Color = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Colors/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}