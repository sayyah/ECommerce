using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.BlogCategories;

public class CreateModel(IBlogCategoryService blogCategoryService) : PageModel
{
    [BindProperty] public BlogCategory BlogCategory { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var result = await blogCategoryService.Add(BlogCategory);
            if (result.Code == 0)
                return RedirectToPage("/BlogCategories/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }
}