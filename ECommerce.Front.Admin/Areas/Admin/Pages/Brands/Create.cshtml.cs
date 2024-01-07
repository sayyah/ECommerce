using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Brands;

public class CreateModel(IBrandService brandService, IImageService imageService, IHostEnvironment environment)
    : PageModel
{
    [BindProperty] public Brand Brand { get; set; }

    [BindProperty] public IFormFile Upload { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (Upload == null)
        {
            Message = "لطفا عکس را انتخاب کنید";
            Code = ServiceCode.Error.ToString();
            return Page();
        }

        var fileName = (await imageService.Upload(Upload, "Images/Brands", environment.ContentRootPath))
            .ReturnData;
        if (fileName == null)
        {
            ModelState.AddModelError("IvalidFileExtention", "فرمت فایل پشتیبانی نمی‌شود.");
            return Page();
        }

        Brand.ImagePath = $"/{fileName[0]}/{fileName[1]}/{fileName[2]}";

        if (ModelState.IsValid)
        {
            var result = await brandService.Add(Brand);
            if (result.Code == 0)
                return RedirectToPage("/Brands/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }
}