using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Pages;

public class AboutUsModel(IBrandService brandService) : PageModel
{
    public List<Brand> Brands { get; set; }

    public async Task OnGetAsync()
    {
        Brands = (await brandService.Load()).ReturnData;
        Brands.RemoveAt(0);
    }
}