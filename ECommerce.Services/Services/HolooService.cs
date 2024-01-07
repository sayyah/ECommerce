namespace ECommerce.Services.Services;

public class HolooService(IHttpService http) : IHolooService
{
    public async Task<string> ConvertHoloo(bool isAllMGroupConvert, string mGroupCode)
    {
        //var resultDialog = await _sweet.FireAsync(new SweetAlertOptions
        //{
        //    Title = "تبدیل از هلو",
        //    Text = "اگر قبلا کالایی را از این گروه یا گروه ها تبدیل کردید، تکراری ذخیره خواهد شد. آیا مایل به انجام هستید؟",
        //    Icon = SweetAlertIcon.Warning,
        //    ShowCancelButton = true,
        //    ConfirmButtonText = "بله، تبدیل کن",
        //    CancelButtonText = "نه، خودم دستی وارد میکنم"
        //});
        //if (resultDialog.Dismiss == DismissReason.Cancel) return false;
        var mCode = isAllMGroupConvert == false ? mGroupCode : "";
        var response = await http.PostAsync("api/Products/ConvertHolooToSunflower", mCode);
        if (response.Code == 0) return "با موفقیت تبدیل شد";
        return response.GetBody();
    }
}