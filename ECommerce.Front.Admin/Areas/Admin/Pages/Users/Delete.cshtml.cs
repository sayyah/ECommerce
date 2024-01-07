using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Users;

public class DeleteModel(IUserService userService) : PageModel
{
    public User User { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await userService.GetById(id);
        if (result.Code == 0)
        {
            User = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Users/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await userService.Delete(id);
            return RedirectToPage("/Users/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}
