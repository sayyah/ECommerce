using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.SlideShows;

public class EditModel(ISlideShowService slideShowService, IHostEnvironment environment, IImageService imageService)
    : PageModel
{
    [BindProperty] public IFormFile Upload { get; set; }
    [BindProperty] public SlideShowViewModel SlideShow { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }

    public async Task OnGet(int id)
    {
        var result = await slideShowService.GetById(id);
        SlideShow = result.ReturnData;
    }

    public async Task<IActionResult> OnPost()
    {
        if (Upload != null)
        {
            var fileName = (await imageService.Upload(Upload, "Images/SlideShows", environment.ContentRootPath))
                .ReturnData;
            if (fileName == null)
            {
                ModelState.AddModelError("IvalidFileExtention", "فرمت فایل پشتیبانی نمی‌شود.");
                return Page();
            }

            SlideShow.ImagePath = $"/{fileName[0]}/{fileName[1]}/{fileName[2]}";
        }

        if (Upload == null && SlideShow.ImagePath == null)
        {
            Message = "لطفا عکس را انتخاب کنید";
            Code = ServiceCode.Error.ToString();
            return Page();
        }

        ModelState.Remove("Upload");
        if (ModelState.IsValid)
        {
            var result = await slideShowService.Edit(SlideShow);
            Message = result.Message;
            Code = result.Code.ToString();
            if (result.Code == 0)
                return RedirectToPage("/SlideShows/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }
}