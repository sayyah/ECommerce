using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.SlideShows;

public class CreateModel(ISlideShowService slideShowService, IImageService imageService, IHostEnvironment environment,
        IProductService productService, ICategoryService categoryService)
    : PageModel
{
    [BindProperty] public SlideShow SlideShow { get; set; }

    [BindProperty] public IFormFile Upload { get; set; }

    //public PaginationViewModel PaginationViewModel { get; set; }
    public ServiceResult<List<ProductIndexPageViewModel>> Products { get; set; }
    public List<CategoryParentViewModel> Categories { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task OnGet(string search)
    {
        var result = await productService.Search(search, 1, 30);
        if (result.Code == ServiceCode.Success)
        {
            Message = result.Message;
            Code = result.Code.ToString();
            Products = result;
        }

        var resultCategory = await categoryService.GetParents();
        Categories = resultCategory.ReturnData;
    }

    public async Task<IActionResult> OnPost(int selectItem)
    {
        if (selectItem == 1)
            SlideShow.CategoryId = null;
        else
            SlideShow.ProductId = null;
        if (Upload == null)
        {
            Message = "لطفا عکس را انتخاب کنید";
            Code = ServiceCode.Error.ToString();
            return Page();
        }

        var fileName = (await imageService.Upload(Upload, "Images/SlideShows", environment.ContentRootPath))
            .ReturnData;
        if (fileName == null)
        {
            ModelState.AddModelError("IvalidFileExtention", "فرمت فایل پشتیبانی نمی‌شود.");
            await Initial();
            return Page();
        }

        SlideShow.ImagePath = $"/{fileName[0]}/{fileName[1]}/{fileName[2]}";

        ModelState.Remove("SlideShow.ImagePath");

        if (ModelState.IsValid)
        {
            var result = await slideShowService.Add(SlideShow);
            if (result.Code == 0)
                return RedirectToPage("/SlideShows/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        await Initial();

        return Page();
    }

    private async Task Initial()
    {
        Products = await productService.Search("", 1, 30);
        var resultCategory = await categoryService.GetParents();
        Categories = resultCategory.ReturnData;
    }

    public async Task<JsonResult> OnGetReturnProducts(string search = "")
    {
        var result = await productService.Search(search, 1, 30);
        if (result.Code == ServiceCode.Success)
        {
            Message = result.Message;
            Code = result.Code.ToString();
        }

        return new JsonResult(result.ReturnData);
    }
}
