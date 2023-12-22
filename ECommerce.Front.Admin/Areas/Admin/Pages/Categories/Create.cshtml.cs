using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Categories;

public class CreateModel : PageModel
{
    private readonly ICategoryService _categoryService;
    private readonly IHostEnvironment _environment;
    private readonly IImageService _imageService;

    public CreateModel(ICategoryService categoryService, IImageService imageService, IHostEnvironment environment)
    {
        _categoryService = categoryService;
        _imageService = imageService;
        _environment = environment;
    }

    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }
    [BindProperty] public IFormFile Upload { get; set; }

    [BindProperty] public Category Category { get; set; }
    public List<CategoryParentViewModel> Categories { get; set; }

    public async Task OnGet()
    {
        Category = new Category();
        var result = await _categoryService.GetParents();
        Categories = result.ReturnData;
    }

    public async Task<IActionResult> OnPost()
    {
        Category.Depth++;
        if (Category.ParentId == 0)
        {
            Category.ParentId = null;
            if (Category.Path == null) Category.Path = Category.Name;
            Category.Depth--;
        }

        if (Upload == null)
        {
            Message = "لطفا عکس را انتخاب کنید";
            Code = ServiceCode.Error.ToString();
            return Page();
        }

        var fileName = (await _imageService.Upload(Upload, "Images/Categories", _environment.ContentRootPath))
            .ReturnData;
        if (fileName == null)
        {
            ModelState.AddModelError("IvalidFileExtention", "فرمت فایل پشتیبانی نمی‌شود.");
            return Page();
        }

        Category.ImagePath = $"/{fileName[0]}/{fileName[1]}/{fileName[2]}";
        ModelState.Remove("Category.ImagePath");

        if (ModelState.IsValid)
        {
            var result = await _categoryService.Add(Category);
            if (result.Code == 0)
                return RedirectToPage("/Categories/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        var resultParent = await _categoryService.GetParents();
        Categories = resultParent.ReturnData;
        return Page();
    }
}