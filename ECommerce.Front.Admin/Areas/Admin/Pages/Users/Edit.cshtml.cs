using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Users;

public class EditModel(IUserService userService) : PageModel
{
    [BindProperty] public User User { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }

    public async Task OnGet(int id)
    {
        var result = await userService.GetById(id);
        User = result.ReturnData;
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var resultUser = await userService.GetById(User.Id);
            var editUser = new User();
            if (resultUser.Code == ServiceCode.Success) editUser = resultUser.ReturnData;
            editUser.UserName = User.UserName;
            editUser.FirstName = User.FirstName;
            editUser.LastName = User.LastName;
            editUser.NationalCode = User.NationalCode;
            editUser.Email = User.Email;
            editUser.Mobile = User.Mobile;
            editUser.PhoneNumber = User.PhoneNumber;
            editUser.State = User.State;
            editUser.IsActive = User.IsActive;
            var result = await userService.Update(editUser);
            Message = result.Message;
            Code = result.Code.ToString();
            if (result.Code == 0)
                return RedirectToPage("/Users/Index",
                    new { area = "Admin", message = result.Message, code = result.Code });
            Message = result.Message == null ? "پیغام خطای غیرمنتظره" : result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", Message);
        }

        return Page();
    }
}
 