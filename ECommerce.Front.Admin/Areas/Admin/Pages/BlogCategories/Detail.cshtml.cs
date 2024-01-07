using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.BlogCategories;

public class DetailModel(IBlogCategoryService blogcategoryService) : PageModel
{
    public BlogCategory BlogCategory { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await blogcategoryService.GetById(id);
        if (result.Code == 0)
        {
            BlogCategory = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/BlogCategories/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}