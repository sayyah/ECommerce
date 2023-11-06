using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Pages;

public class UserProfileModel : PageModel
{
    private readonly ICityService _cityService;
    private readonly IPurchaseOrderService _purchaseOrderService;
    private readonly IStateService _stateService;
    private readonly IUserService _userService;

    public UserProfileModel(ICityService cityService, IStateService stateService, IUserService userService,
        IPurchaseOrderService purchaseOrderService)
    {
        _stateService = stateService;
        _userService = userService;
        _purchaseOrderService = purchaseOrderService;
        _cityService = cityService;
    }


    [BindProperty] public User UserInformation { get; set; }
    public List<PurchaseListViewModel> PurchaseOrders { get; set; }
    public ServiceResult<List<State>> StateList { get; set; }
    [BindProperty] public List<City> CityList { get; set; }
    public string Message { get; set; }
    public string Code { get; set; }

    public async Task OnGet()
    {
        await Initial();
    }

    public async Task<IActionResult> OnPostEdit()
    {
        var resultUser = await _userService.GetUser();
        var editUser = new User();
        if (resultUser.Code == ServiceCode.Success) editUser = resultUser.ReturnData;

        editUser.CityId = UserInformation.CityId;
        editUser.StateId = UserInformation.StateId;
        editUser.FirstName = UserInformation.FirstName;
        editUser.LastName = UserInformation.LastName;
        editUser.NationalCode = UserInformation.NationalCode;
        editUser.Email = UserInformation.Email;
        editUser.CompanyName = UserInformation.CompanyName;
        editUser.Mobile = UserInformation.Mobile;
        var result = await _userService.Update(editUser);
        Message = result.Message;
        Code = result.Code.ToString();
        await Initial();
        return Page();
    }

    public async Task<IActionResult> OnGetChangePassword(string oldPass, string newPass, string newConPass)
    {
        var result = await _userService.ChangePassword(oldPass, newPass, newConPass);
        return new JsonResult(result);
    }

    private async Task Initial()
    {
        var resultUser = await _userService.GetUser();
        if (resultUser.Code == ServiceCode.Success) UserInformation = resultUser.ReturnData;
        var resultPurchaseOrder = await _purchaseOrderService.PurchaseList(UserInformation.Id);
        if (resultPurchaseOrder.Code == ServiceCode.Success) PurchaseOrders = resultPurchaseOrder.ReturnData;
        StateList = await _stateService.Load();
        var cityServiceResponse = await _cityService.LoadAllCity();
        CityList = cityServiceResponse.ReturnData;
    }

    public IActionResult OnGetFactorPrint(long orderId)
    {
        return RedirectToPage("InvoiceReportPrint", new { orderId });
    }
}