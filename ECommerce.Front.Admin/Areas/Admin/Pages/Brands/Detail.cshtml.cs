using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Brands;

public class DetailModel(IBrandService brandService) : PageModel
{
    public Brand Brand { get; set; }

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
}