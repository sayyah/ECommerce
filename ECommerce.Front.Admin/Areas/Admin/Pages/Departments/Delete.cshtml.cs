using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Departments;

public class DeleteModel(IDepartmentService departmentService) : PageModel
{
    public Department Department { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await departmentService.GetById(id);
        if (result.Code == 0)
        {
            Department = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Departments/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await departmentService.Delete(id);
            return RedirectToPage("/Departments/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}