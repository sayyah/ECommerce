using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.SlideShows;

public class DetailModel(ISlideShowService slideShowService) : PageModel
{
    public SlideShowViewModel SlideShow { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await slideShowService.GetById(id);
        if (result.Code == 0)
        {
            SlideShow = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/SlideShows/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}