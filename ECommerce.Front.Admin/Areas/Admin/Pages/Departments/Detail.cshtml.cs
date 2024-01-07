using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Departments;

public class DetailModel(IDepartmentService departmentService) : PageModel
{
    public Department Department { get; set; }

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
}