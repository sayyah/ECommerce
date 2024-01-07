using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Brands;

public class DeleteModel(IBrandService brandService) : PageModel
{
    public Brand Brand { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await brandService.GetById(id);
        if (result.Code == 0)
        {
            Brand = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Brands/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await brandService.Delete(id);
            return RedirectToPage("/Brands/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}