using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Employees;

public class DetailModel(IEmployeeService employeeService, IDepartmentService departmentService)
    : PageModel
{
    public Employee Employee { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await employeeService.GetById(id);
        if (result.Code == 0)
        {
            Employee = result.ReturnData;
            Employee.Department = departmentService.GetById((int)result.ReturnData.DepartmentId).Result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Employees/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}