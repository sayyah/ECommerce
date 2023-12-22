using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Departments;

public class IndexModel : PageModel
{
    private readonly IDepartmentService _departmentService;

    public IndexModel(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    public ServiceResult<List<Department>> Departments { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(string search = "", int pageNumber = 1, int pageSize = 10,
        string message = null, string code = null)
    {
        Message = message;
        Code = code;
        var result = await _departmentService.Load(search, pageNumber, pageSize);
        if (result.Code == ServiceCode.Success)
        {
            if (Message != null)
            {
                Message = Message;
                Code = Code;
            }
            else
            {
                Message = result.Message;
                Code = result.Code.ToString();
            }

            Code = result.Code.ToString();
            Departments = result;
            return Page();
        }

        return RedirectToPage("/index", new { message = result.Message, code = result.Code.ToString() });
    }
}