﻿using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Pages;

public class RegisterModel(IUserService userService, ICityService cityService, IStateService stateService)
    : PageModel
{
    [BindProperty] public RegisterViewModel RegisterViewModel { get; set; } = new();

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }
    [BindProperty] public List<State> StateList { get; set; }
    [BindProperty] public List<City> CityList { get; set; }

    public async Task<IActionResult> OnGet(string mobile, int confirmCode)
    {
        if (string.IsNullOrEmpty(mobile)) return RedirectToPage("index");
        RegisterViewModel.Username = mobile;
        RegisterViewModel.Mobile = mobile;
        RegisterViewModel.ConfirmCode = confirmCode;
        await Load();
        return Page();
    }

    private async Task Load()
    {
        StateList = (await stateService.Load()).ReturnData;
        var cityServiceResponse = await cityService.LoadAllCity();
        CityList = cityServiceResponse.ReturnData;
    }

    public async Task<IActionResult> OnPostRegister()
    {
        await Load();
        if (!await userService.GetVerificationByNationalId(RegisterViewModel.NationalCode))

        {
            Message = "کد ملی نامعتبر می باشد";
            Code = "Error";
            return Page();
        }

        var codeConfirm = GenerateCode(RegisterViewModel.Mobile);
        if (RegisterViewModel.ConfirmCode != codeConfirm)
        {
            Message = "کد تایید نامعتبر می باشد";
            Code = "Error";
            return Page();
        }

        if (!RegisterViewModel.IsRole)
        {
            //          !qa@ws#ed123
            //          !qa@ws#ed123123
            Message = "لطفا ایتدا قوانین و مقررارت را تایید کنید";
            Code = "Error";
            return Page();
        }

        ModelState.Remove("RegisterViewModel.Email");
        ModelState.Remove("RegisterViewModel.Username");

        if (!ModelState.IsValid) return Page();
        switch (RegisterViewModel.CompanyType)
        {
            case 10:
                RegisterViewModel.CompanyTypeName = "رستوران";
                break;
            case 11:
                RegisterViewModel.CompanyTypeName = "کافی شاپ";
                break;
            case 9:
                RegisterViewModel.CompanyTypeName = "هتل";
                break;
            case 16:
                RegisterViewModel.CompanyTypeName = "تالار";
                break;
            case 21:
                RegisterViewModel.CompanyTypeName = "شرکت";
                break;
            case 15:
                RegisterViewModel.CompanyTypeName = "فروشگاه";
                break;
        }

        RegisterViewModel.Username = RegisterViewModel.Mobile;
        RegisterViewModel.Email = "boloorico@gmail.com";
        RegisterViewModel.IsHaveEmail = false;

        var result = await userService.Register(RegisterViewModel);
        if (result.Code > 0)
        {
            Message = result.Message;
            Code = result.Code.ToString();
            return Page();
        }

        return RedirectToPage("/index");
    }

    public async Task<IActionResult> OnGetSendSms(string username)
    {
        var code = GenerateCode(username);
        if (code == 0) return new JsonResult(null);
        var smsResponsModel = await userService.SendAuthenticationSms(username, code.ToString());
        return new JsonResult(smsResponsModel);
    }

    public int GenerateCode(string mobile)
    {
        if (mobile == null) return 0;
        int number;
        if ((mobile.Length != 11) & (mobile.Length != 10)) return 0;
        if (mobile.Substring(0, 1) != "0") mobile = "0" + mobile;
        if (mobile.Substring(0, 2) != "09") return 0;
        if (!int.TryParse(mobile.Substring(1, 9), out number)) return 0;
        var result = getSumResult(mobile.Substring(10, 1), mobile.Substring(4, 1));
        result = result + getSumResult(mobile.Substring(9, 1), mobile.Substring(5, 1));
        result = result + getSumResult(mobile.Substring(8, 1), mobile.Substring(6, 1));
        result = result + getSumResult(mobile.Substring(7, 1), mobile.Substring(3, 1));
        int.TryParse(result, out number);
        return number;
    }

    private string getSumResult(string num1, string num2)
    {
        int number1, number2;
        int.TryParse(num1, out number1);
        int.TryParse(num2, out number2);
        int sum;
        do
        {
            sum = number1 + number2;
            if (sum > 10)
            {
                number1 = sum - 10;
                number2 = number1 > 4 ? 1 : 0;
            }
        } while (sum > 10);

        return sum + "";
    }
}