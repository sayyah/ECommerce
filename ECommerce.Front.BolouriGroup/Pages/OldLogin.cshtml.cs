using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Pages;

public class OldLoginModel(IUserService userService) : PageModel
{
    [BindProperty] public RegisterViewModel RegisterViewModel { get; set; }

    [BindProperty] public LoginViewModel LoginViewModel { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    [TempData] public string ReturnUrl { get; set; }

    public void OnGet(string returnUrl = null)
    {
        if (string.IsNullOrEmpty(returnUrl)) returnUrl = "/";
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostSubmit()
    {
        var s = TempData["ReturnUrl"];
        //ReturnUrl = "/Shop/Coffee/shop/equipment/Hot.bar/Coffee.makers";
        var result = await userService.Login(LoginViewModel);
        if (result.Code == 0)
            return RedirectToPage("/Index");

        Message = result.Message;
        Code = result.Code.ToString();

        return Page();
    }

    public async Task<IActionResult> OnPostRegister()
    {
        if (!ModelState.IsValid) return Page();
        var result = await userService.Register(RegisterViewModel);
        Message = result.Message;
        Code = result.Code.ToString();
        if (result.Code == 0) return RedirectToPage("/Index");
        return Page();
    }

    public async Task<JsonResult> OnGetSecondsLeft(string username)
    {
        var checkUsernameResult = await CheckUsername(username);
        return new JsonResult(checkUsernameResult);
    }

    private async Task<ServiceResult<int?>> CheckUsername(string username)
    {
        var checkUsernameResult = await userService.GetSecondsLeftConfirmCodeExpire(username);

        if (checkUsernameResult.Code == ServiceCode.Error)
            return new ServiceResult<int?>
            {
                Message = "نام کاربری موجود نمی باشد",
                Code = ServiceCode.Error
            };

        if (checkUsernameResult.ReturnData > 0)
            return new ServiceResult<int?>
            {
                Message = $"{checkUsernameResult.ReturnData}ثانیه باقی مانده",
                Code = ServiceCode.Info,
                ReturnData = checkUsernameResult.ReturnData
            };

        return new ServiceResult<int?>
        {
            Message = "کاربر موجود و امکان ارسال پیامک وجود دارد",
            Code = ServiceCode.Success
        };
    }

    public async Task<IActionResult> OnGetSendSms(string username)
    {
        var checkUsernameResult = await CheckUsername(username);
        if (checkUsernameResult.Code != ServiceCode.Success) return Page();

        var randomCode = new Random();
        var code = randomCode.Next(100000000);
        if (code < 10000000) code = code + 10000000;
        var smsResponsModel = await userService.SendAuthenticationSms(username, code.ToString());
        if (smsResponsModel.Status != 1)
        {
            Message = smsResponsModel.Message;
            Code = "Error";
            return Page();
        }

        var result = await userService.SetConfirmCodeByUsername(username, code.ToString());
        if (!result.ReturnData)
        {
            Message = "نام کاربری صحیح نمی باشد";
            Code = "Error";
            return Page();
        }

        Message = "130 sec";
        Code = "Info";
        return Page();
    }

    public async Task<JsonResult> OnGetUserLoginSubmit(string username, string password)
    {
        var _loginViewModel = new LoginViewModel();
        _loginViewModel.Username = username;
        _loginViewModel.Password = password;
        ServiceResult<LoginViewModel?> result = await userService.Login(_loginViewModel);
        return new JsonResult(result);
    }
}