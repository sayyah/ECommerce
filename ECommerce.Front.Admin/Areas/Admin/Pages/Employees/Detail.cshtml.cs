using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Areas.Admin.Pages.Employees;

public class DetailModel : PageModel
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;

    public DetailModel(IEmployeeService employeeService, IDepartmentService departmentService)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
    }

    public Employee Employee { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await _employeeService.GetById(id);
        if (result.Code == 0)
        {
            Employee = result.ReturnData;
            Employee.Department = _departmentService.GetById((int)result.ReturnData.DepartmentId).Result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Employees/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}