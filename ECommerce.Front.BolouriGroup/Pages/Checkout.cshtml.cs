using System.Security.Cryptography;
using System.Text;
using ECommerce.Front.BolouriGroup.Models;
using ECommerce.Services.IServices;
using PersianDate.Standard;

namespace ECommerce.Front.BolouriGroup.Pages;

public class CheckoutModel : PageModel
{
    private readonly ICartService _cartService;
    private readonly ICityService _cityService;
    private readonly IDiscountService _discountService;
    private readonly IPurchaseOrderService _purchaseOrderService;
    private readonly ISendInformationService _sendInformationService;
    private readonly IStateService _stateService;
    private readonly IConfiguration _configuration;

    [BindProperty]
    public List<State> StateList { get; set; }

    [BindProperty]
    public List<City> CityList { get; set; }

    [BindProperty]
    public SendInformation SendInformation { get; set; }

    [BindProperty]
    public List<SendInformation> SendInformationList { get; set; }

    [TempData]
    public string Message { get; set; }

    [TempData]
    public string Code { get; set; }
    public int SumPrice { get; set; }
    public ServiceResult<Discount> DiscountResult { get; set; }

    public CheckoutModel(
        ICartService cartService,
        ICityService cityService,
        ISendInformationService sendInformationService,
        IStateService stateService,
        IPurchaseOrderService purchaseOrderService,
        IDiscountService discountService,
        IConfiguration configuration
    )
    {
        _cityService = cityService;
        _sendInformationService = sendInformationService;
        _stateService = stateService;
        _purchaseOrderService = purchaseOrderService;
        _cartService = cartService;
        _discountService = discountService;
        _configuration = configuration;
    }

    public async Task OnGet(string message, string code)
    {
        await Initial();
        Message = message;
        Code = code;
    }

    private async Task Initial()
    {
        StateList = (await _stateService.Load()).ReturnData;
        var cityServiceResponse = await _cityService.LoadAllCity();
        CityList = cityServiceResponse.ReturnData;
        SendInformationList = (await _sendInformationService.Load()).ReturnData;
        var resultCart = await _cartService.CartListFromServer(true);
        if (resultCart.Code > 0)
        {
            Message = resultCart.Message;
            Code = resultCart.Code.ToString();
        }

        var cart = resultCart.ReturnData;
        var tempSumPrice = cart.Sum(x => x.SumPrice);
        SumPrice = Convert.ToInt32(tempSumPrice);
    }

    public async Task<JsonResult> OnGetSendInformationLoad(int id)
    {
        var result = await _cityService.Load(id);
        var ret = "";
        foreach (var city in result.ReturnData)
            ret += $"<option value='{city.Id}'>{city.Name}</option>";
        return new JsonResult(ret);
    }

    public async Task<JsonResult> OnGetDiscount(string discountCode)
    {
        await Initial();
        var sumPriceResult = await DiscountCalculate(discountCode, SumPrice);

        return new JsonResult(sumPriceResult);
    }

    public async Task<JsonResult> OnGetRemoveDiscount()
    {
        await Initial();
        return new JsonResult(SumPrice);
    }

    public async Task<JsonResult> OnPostEditAddress()
    {
        var result = await _sendInformationService.Edit(SendInformation);
        var list = (await _sendInformationService.Load()).ReturnData;
        return new JsonResult(
            new
            {
                result.Code,
                result.Message,
                Data = list,
            }
        );
    }

    private async Task<VerifyResultData> DiscountCalculate(string discountCode, int sumPrice)
    {
        DiscountResult = await _discountService.GetByCode(discountCode);
        var discount = DiscountResult.ReturnData;
        VerifyResultData resultData = new();
        if (DiscountResult.Code > 0 || DiscountResult.Status != 200)
        {
            resultData.SumPrice = sumPrice;
            resultData.Succeed = false;
            resultData.Description = "تخفیف مورد نظر یافت نشد";
            return resultData;
        }
        if (!DiscountResult.ReturnData.IsActive)
        {
            resultData.SumPrice = sumPrice;
            resultData.Succeed = false;
            resultData.Description = "تخفیف مورد نظر دیگر فعال نیست";
            return resultData;
        }

        if (DiscountResult.ReturnData.StartDate?.Date > DateTime.Now.Date)
        {
            resultData.SumPrice = sumPrice;
            resultData.Succeed = false;
            resultData.Description =
                $"تخفیف مورد نظر هنوز شروع نشده است، لطفا بعد از تاریخ {DiscountResult.ReturnData.StartDate.ToFa()} دوباره تلاش کنید";
            return resultData;
        }

        if (DiscountResult.ReturnData.EndDate?.Date < DateTime.Now.Date)
        {
            resultData.SumPrice = sumPrice;
            resultData.Succeed = false;
            resultData.Description = "تخفیف مورد نظر منقضی شده است";
            return resultData;
        }

        var sumPriceAfterDiscount = 0;
        if (discount.Amount is > 0)
        {
            sumPriceAfterDiscount = sumPrice - (int)discount.Amount;
            if (sumPriceAfterDiscount < 0)
                sumPriceAfterDiscount = 0;
        }
        else
        {
            if (discount.Percent != null)
                sumPriceAfterDiscount = sumPrice - (int)(discount.Percent.Value / 100 * sumPrice);
        }

        if (sumPriceAfterDiscount <= 0)
        {
            resultData.SumPrice = sumPrice;
            resultData.Succeed = false;
            resultData.Description = "تخفیف غیرمجاز می‌باشد.";
            return resultData;
        }

        sumPrice = sumPriceAfterDiscount;
        resultData.SumPrice = sumPrice;
        resultData.Succeed = true;
        resultData.Description = "تخفیف بعد از پرداخت در فاکتور شما لحاظ خواهد شد";
        return resultData;
    }

    public async Task<IActionResult> OnPost(string Portal, int PostPrice, string discountCode)
    {
        await Initial();
        ModelState.Remove("discountCode");
        var returnAction = "melisuccess";
        var url = $"https://{Request.Host}{Request.PathBase}/";
        SendInformation.UserId = Convert.ToInt32(
            User.Claims.FirstOrDefault(c => c.Type == "id")?.Value
        );
        var resultSendInformation = ServiceCode.Success;
        if (SendInformation.Id == 0)
        {
            ModelState.Remove("SendInformation.Id");
            if (ModelState.IsValid)
            {
                var result = await _sendInformationService.Add(SendInformation);
                resultSendInformation = result.Code;
                Message = result.Message;
                SendInformation = result.ReturnData;
            }
            else
            {
                Message = "لطفا یک آدرس جدید وارد کنید یا یک آدرس را انتخاب کنید";
                Code = "Error";
                return Page();
            }
        }
        else
        {
            var r = await _sendInformationService.Find(SendInformation.Id);
            SendInformation = r.ReturnData;
        }

        var resultCart = await _cartService.CartListFromServer();
        if (resultCart.Code > 0)
        {
            Message = resultCart.Message;
            Code = resultCart.Code.ToString();
        }

        var cart = resultCart.ReturnData;
        var tempSumPrice = cart.Sum(x => x.SumPrice);

        var discountResult = await DiscountCalculate(discountCode, Convert.ToInt32(tempSumPrice));
        SumPrice = discountResult.SumPrice;
        if (SumPrice >= 50000000)
        {
            Message = "مبلغ سفارش نمی تواند بیشتر از 50 میلیون تومان باشد";
            Code = "Error";
            return Page();
        }

        var purchaseOrder = (await _purchaseOrderService.GetByUserId()).ReturnData;
        purchaseOrder.Amount = tempSumPrice;
        purchaseOrder.SendInformationId = SendInformation.Id;
        if (
            DiscountResult.Code == 0
            && DiscountResult.Status == 200
            && DiscountResult.ReturnData.IsActive
            && (
                DiscountResult.ReturnData.StartDate?.Date <= DateTime.Now.Date
                || DiscountResult.ReturnData.StartDate == null
            )
            && (
                DiscountResult.ReturnData.EndDate?.Date >= DateTime.Now.Date
                || DiscountResult.ReturnData.EndDate == null
            )
        )
        {
            purchaseOrder.DiscountId = DiscountResult.ReturnData.Id;
            purchaseOrder.DiscountAmount = (int)tempSumPrice - SumPrice;
        }

        if (resultSendInformation == 0)
        {
            var description = "";
            switch (Portal)
            {
                //case "zarinpal":
                //    returnAction = "ZarinPalSuccess";
                //    description = "خرید تستی ";
                //    purchaseOrder.OrderGuid = Guid.NewGuid();
                //    byte[] gb1 = purchaseOrder.OrderGuid.ToByteArray();
                //    purchaseOrder.OrderId = BitConverter.ToInt64(gb1, 0);
                //    var paymentZarinpal = await new Payment(SumPrice).PaymentRequest(description, url + returnAction + "?Factor=" + purchaseOrder.Id);
                //    if (paymentZarinpal.Status == 100)
                //    {
                //        await _purchaseOrderService.Edit(purchaseOrder);
                //        return Redirect(paymentZarinpal.Link);
                //    }
                //    return RedirectToPage("Error");
                case "sadad":
                    SumPrice *= 10;
                    purchaseOrder.OrderGuid = Guid.NewGuid();
                    var gb = purchaseOrder.OrderGuid.ToByteArray();
                    purchaseOrder.OrderId = BitConverter.ToInt64(gb, 0);
                    var date = DateTime.Now.ToString("yyyyMMdd");
                    var time = DateTime.Now.ToString("HHmmss");
                    long merchantId = _configuration.GetValue<long>(
                        "SiteSettings:SanadSettings:merchantId"
                    );
                    string terminalId = _configuration.GetValue<string>(
                        "SiteSettings:SanadSettings:terminalId"
                    );
                    string terminalKey = _configuration.GetValue<string>(
                        "SiteSettings:SanadSettings:terminalKey"
                    );
                    var dataBytes = Encoding
                        .UTF8
                        .GetBytes($"{terminalId};{purchaseOrder.OrderId};{SumPrice}");
                    var symmetric = SymmetricAlgorithm.Create("TripleDes");
                    symmetric.Mode = CipherMode.ECB;
                    symmetric.Padding = PaddingMode.PKCS7;
                    var encryptor = symmetric.CreateEncryptor(
                        Convert.FromBase64String(terminalKey),
                        new byte[8]
                    );
                    var signData = Convert.ToBase64String(
                        encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length)
                    );

                    var ipgUri = "https://sadad.shaparak.ir/api/v0/Request/PaymentRequest";
                    var data = new
                    {
                        MerchantId = merchantId,
                        TerminalId = terminalId,
                        Amount = SumPrice,
                        purchaseOrder.OrderId,
                        LocalDateTime = DateTime.Now,
                        ReturnUrl = url + returnAction,
                        signData
                    };

                    var res = await CallApi<PayResultData>(ipgUri, data);

                    if (res.ResCode == "0")
                    {
                        await _purchaseOrderService.Edit(purchaseOrder);
                        return Redirect($"https://sadad.shaparak.ir/Purchase?Token={res.Token}");
                    }

                    return RedirectToPage("Error", new { message = res.Description });
            }

            Code = "Error";
            Message = "خطا هنگام اتصال به درگاه بانکی";
        }
        else
        {
            if (string.IsNullOrEmpty(Message) || Message.Equals("\n\r"))
            {
                Message = "لطفا اطلاعات آدرس را تکمیل کنید یا از لیست یک آدرس را انتخاب کنید";
                Code = "Info";
            }
        }

        Code = resultSendInformation.ToString();
        return Page();
    }

    public static async Task<T> CallApi<T>(string apiUrl, object value)
    {
        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            var response = await client.PostAsJsonAsync(apiUrl, value);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<T>();
            return default;
        }
    }
}
