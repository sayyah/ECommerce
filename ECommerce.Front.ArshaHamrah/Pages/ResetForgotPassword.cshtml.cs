using ECommerce.Entities.ViewModel;
using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerce.Front.ArshaHamrah.Pages;

public class ResetForgotPasswordModel : PageModel
{
    private readonly IUserService _userService;

    public ResetForgotPasswordModel(IUserService userService)
    {
        _userService = userService;
    }

    [BindProperty] public string Token { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public void OnGet(string token)
    {
        Token = token;
    }

    public async Task<IActionResult> OnPost(string password, string conpass, string token, string email)
    {
        var resetForgotPasswordViewModel = new ResetForgotPasswordViewModel
        {
            Password = password,
            ConPass = conpass,
            PasswordResetToken = token,
            Email = email
        };
        var result = await _userService.ChangeForgotPassword(resetForgotPasswordViewModel);
        return RedirectToPage("index");
    }
}