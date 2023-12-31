﻿using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Employees;

public class EditModel : PageModel
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;

    public EditModel(IEmployeeService employeeService, IDepartmentService departmentService)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
    }

    [BindProperty] public Employee Employee { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }
    public SelectList Departments { get; set; }

    public async Task OnGet(int id)
    {
        var result = await _employeeService.GetById(id);
        Employee = result.ReturnData;
        await Init();
    }

    private async Task Init() {
        var departments = (await _departmentService.GetAll()).ReturnData;
        Departments = new SelectList(departments, nameof(Department.Id), nameof(Department.Title));
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var result = await _employeeService.Edit(Employee);
            Message = result.Message;
            Code = result.Code.ToString();
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