using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Sizes;

public class DeleteModel(ISizeService sizeService) : PageModel
{
    public Size Size { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await sizeService.GetById(id);
        if (result.Code == 0)
        {
            Size = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Sizes/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await sizeService.Delete(id);
            return RedirectToPage("/Sizes/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}