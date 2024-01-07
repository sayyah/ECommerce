using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Categories;

public class DetailModel(ICategoryService categoryService) : PageModel
{
    public Category Category { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await categoryService.GetById(id);
        if (result.Code == 0)
        {
            Category = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Categories/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}