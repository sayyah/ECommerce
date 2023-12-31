using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.SlideShows;

public class DeleteModel : PageModel
{
    private readonly ISlideShowService _slideShowService;

    public DeleteModel(ISlideShowService slideShowService)
    {
        _slideShowService = slideShowService;
    }

    public SlideShowViewModel SlideShow { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await _slideShowService.GetById(id);
        if (result.Code == 0)
        {
            SlideShow = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/SlideShows/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await _slideShowService.Delete(id);
            return RedirectToPage("/SlideShows/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}