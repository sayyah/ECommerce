using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Areas.Admin.Pages.Brands;

public class DetailModel : PageModel
{
    private readonly IBrandService _brandService;

    public DetailModel(IBrandService brandService)
    {
        _brandService = brandService;
    }

    public Brand Brand { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await _brandService.GetById(id);
        if (result.Code == 0)
        {
            Brand = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Brands/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}