using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Employees;

public class CreateModel(IEmployeeService employeeService, IDepartmentService departmentService)
    : PageModel
{
    [BindProperty] public Employee Employee { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public SelectList Departments { get; set; }

    public async Task OnGet()
    {
        await Init();
    }

    private async Task Init()
    {
        var departments = (await departmentService.GetAll()).ReturnData;
        Departments = new SelectList(departments, nameof(Department.Id), nameof(Department.Title));
    }

    public async Task<IActionResult> OnPost()
    {
        await Init();
        if (ModelState.IsValid)
        {
            var result = await employeeService.Add(Employee);
            if (result.Code == 0)
                return RedirectToPage("/Employees/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }
}