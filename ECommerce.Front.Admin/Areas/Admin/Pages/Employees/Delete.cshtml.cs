using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Employees;

public class DeleteModel(IEmployeeService employeeService) : PageModel
{
    public Employee Employee { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await employeeService.GetById(id);
        if (result.Code == 0)
        {
            Employee = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Employees/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await employeeService.Delete(id);
            return RedirectToPage("/Employees/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}