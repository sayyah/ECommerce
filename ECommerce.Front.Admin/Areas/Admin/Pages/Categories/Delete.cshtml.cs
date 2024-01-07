using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Categories;

public class DeleteModel(ICategoryService categoryService) : PageModel
{
    public Category Category { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

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

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await categoryService.Delete(id);
            return RedirectToPage("/Categories/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}