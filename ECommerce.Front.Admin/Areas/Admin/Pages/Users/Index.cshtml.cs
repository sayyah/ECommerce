using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Users;

public class IndexModel(IUserService userService) : PageModel
{
    public ServiceResult<List<UserListViewModel>> Users { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }
    [BindProperty] public bool? IsCollegue { get; set; }
    [BindProperty] public bool? IsActive { get; set; }

    public async Task<IActionResult> OnGet(string search = "", int pageNumber = 1, int pageSize = 10,
        string message = null, string code = null, bool? iscollegue = null, bool? isactive = null)
    {
        Message = message;
        Code = code;
        IsCollegue = iscollegue;
        IsActive = isactive;
        var result =
            await userService.UserList(search, pageNumber, pageSize, isActive: isactive, isColleague: iscollegue);
        if (result.Code == ServiceCode.Success)
        {
            result.PaginationDetails.Address = "/Users/Index";
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

            Users = result;
            return Page();
        }

        return RedirectToPage("/index", new { message = result.Message, code = result.Code.ToString() });
    }
}
