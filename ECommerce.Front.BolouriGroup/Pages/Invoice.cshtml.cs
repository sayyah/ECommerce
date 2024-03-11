using System.Security.Cryptography;
using System.Text;
using ECommerce.Front.BolouriGroup.Models;
using ECommerce.Services.IServices;
using Hangfire;
using PersianDate.Standard;
using ZarinpalSandbox;

namespace ECommerce.Front.BolouriGroup.Pages;

public class InvoiceModel(IPurchaseOrderService purchaseOrderService,
        IUserService userService,
        IConfiguration configuration)
    : PageModel
{
    [BindProperty]
    public long OrderId { get; set; }

    [TempData]
    public string Message { get; set; }

    [TempData]
    public string Code { get; set; }
    public PurchaseOrder PurchaseOrder { get; set; }
    public string Refid { get; set; }
    public string SystemTraceNo { get; set; }
    public int OrderDetailsDiscount { get; set; }

    public async Task<ActionResult> OnGet(PurchaseResult result)
    {
        return await pay(result);
    }

    public async Task<ActionResult> OnGetPayZarinpal(string factor, string status, string authority)
    {
        if (
            string.IsNullOrEmpty(status) == false
            && string.IsNullOrEmpty(authority) == false
            && string.IsNullOrEmpty(factor) == false
            && status.ToLower() == "ok"
        )
        {
            var resultOrder = await purchaseOrderService.GetByUserId();
            PurchaseOrder = resultOrder.ReturnData;
            var amount = Convert.ToInt32(PurchaseOrder.Amount);

            foreach (var item in PurchaseOrder.PurchaseOrderDetails!)
            {
                OrderDetailsDiscount = OrderDetailsDiscount +
                                       ((int)item.DiscountAmount! * item.Quantity);
            }
            amount = amount - OrderDetailsDiscount!;

            if (PurchaseOrder.DiscountId != null && PurchaseOrder.Discount != null)
            {
                if (PurchaseOrder.Discount.Amount != null && PurchaseOrder.Discount.Amount > 0)
                {
                    amount -= (int)PurchaseOrder.Discount.Amount;
                    if (amount < 0)
                        amount = 0;
                }
                else
                {
                    if (PurchaseOrder.Discount.Percent != null)
                        amount -= (int)(PurchaseOrder.Discount.Percent.Value / 100 * amount);
                }
            }
            else
            {
                PurchaseOrder.DiscountAmount = 0;
            }

            var statusInt = await new Payment(amount).Verification(authority);
            switch (statusInt.Status)
            {
                case -1:
                    Message = "اطلاعات ارسال شده ناقص است.";
                    break;
                case -2:
                    Message = "مشکل نامشخص در درگاه پرداخت با کد " + statusInt.Status;
                    break;
                case -11:
                    Message = "درخواست مورد نظر یافت نشد.";
                    break;
                case -22:
                    Message = "تراکنش ناموفق می باشد.";
                    break;
                case -33:
                    Message = "مبلغ تراکنش با مبلغ پرداخت شده مطابقت ندارد.";
                    break;
                case 100:
                case 101:
                    //Success
                    Refid = statusInt.RefId.ToString();
                    OrderId = PurchaseOrder.OrderId;
                    PurchaseOrder.PaymentDate = DateTime.Now;
                    PurchaseOrder.Transaction = new Transaction
                    {
                        RefId = Refid,
                        Amount = amount,
                        UserId = resultOrder.ReturnData.UserId
                    };
                    var result = await purchaseOrderService.Pay(PurchaseOrder);

                    if (result.Code == ServiceCode.Error)
                    {
                        Message =
                            "مشکل در هنگام ثبت سفارش. لطفا با پشتیبانی سایت تماس حاصل فرمایید.";
                        Code = result.Code.ToString();
                    }
                    else if (result.Code == ServiceCode.Success)
                    {
                        var numbers = configuration.GetSection("InvoiceNumbers:Numbers").Get<string[]>();
                        var job = BackgroundJob.Enqueue(
                            () => userService.SendInvoiceSms(
                                                        result.Message ?? "",
                                                        numbers,
                                                        DateTime.Now.ToString("MM/dd/yyyy")) );
                        Code = result.Code.ToString();
                        Message = "سفارش شما با موفقیت ثبت شد";
                    }

                    return Page();
            }
        }

        return RedirectToPage("Error", new { message = "مشکل در درگاه پرداخت" });
    }

    private async Task<ActionResult> pay(PurchaseResult result)
    {
        var dataBytes = Encoding.UTF8.GetBytes(result.Token);
        var symmetric = TripleDES.Create();
        symmetric.Mode = CipherMode.ECB;
        symmetric.Padding = PaddingMode.PKCS7;
        string? merchantKey = configuration.GetValue<string>(
            "terminalKey"
        );
        var encryptor = symmetric.CreateEncryptor(
            Convert.FromBase64String(merchantKey),
            new byte[8]
        );

        var signedData = Convert.ToBase64String(
            encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length)
        );

        var data = new { token = result.Token, SignData = signedData };

        var resultOrder = await purchaseOrderService.GetByUserId();
        PurchaseOrder = resultOrder.ReturnData;
        var amount = Convert.ToInt32(PurchaseOrder.Amount);
        if (PurchaseOrder.DiscountId != null && PurchaseOrder.Discount != null)
        {
            if (PurchaseOrder.Discount.Amount is > 0)
            {
                amount -= (int)PurchaseOrder.Discount.Amount;
                if (amount < 0)
                    amount = 0;
            }
            else
            {
                if (PurchaseOrder.Discount.Percent != null)
                    amount -= (int)(PurchaseOrder.Discount.Percent.Value / 100 * amount);
            }
        }
        else
        {
            PurchaseOrder.DiscountAmount = 0;
        }

        var ipgUri = "https://sadad.shaparak.ir/api/v0/Advice/Verify";

        var res = CallApi<VerifyResultData>(ipgUri, data);
        if (res != null && res.Result != null)
            if (res.Result.ResCode == "0")
            {
                OrderId = PurchaseOrder.OrderId;
                result.VerifyResultData = res.Result;
                res.Result.Succeed = true;
                SystemTraceNo = res.Result.SystemTraceNo;
                PurchaseOrder.PaymentDate = DateTime.Now;
                PurchaseOrder.Transaction = new Transaction
                {
                    RefId = res.Result.RetrivalRefNo,
                    Amount = amount,
                    UserId = resultOrder.ReturnData.UserId
                };
                var resulPay = await purchaseOrderService.Pay(PurchaseOrder);

                if (resulPay.Code == ServiceCode.Error)
                {
                    Message = "مشکل در هنگام ثبت سفارش. لطفا با پشتیبانی سایت تماس حاصل فرمایید.";
                    Code = resulPay.Code.ToString();
                }
                else if (resulPay.Code == ServiceCode.Success)
                {

                    var numbers = configuration.GetSection("InvoiceNumbers:Numbers").Get<string[]>();
                    var job = BackgroundJob.Enqueue(
                        ()=> userService.SendInvoiceSms(
                        resulPay.Message ?? "",
                        numbers,
                        DateTime.Now.ToFa(("MM/dd/yyyy"))
                        ));
              
                    Code = resulPay.Code.ToString();
                    Message = "سفارش شما با موفقیت ثبت شد";
                }

                Refid = res.Result.RetrivalRefNo;
                return Page();
            }

        return RedirectToPage(
            "Error",
            new { message = "مشکل در درگاه پرداخت " + res.Result.Description }
        );
    }

    public IActionResult OnGetFactorPrint(long orderId)
    {
        return RedirectToPage("InvoiceReportPrint", new { orderId });
    }

    public static async Task<T> CallApi<T>(string apiUrl, object value)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri(apiUrl);
        client.DefaultRequestHeaders.Accept.Clear();
        var response = await client.PostAsJsonAsync(apiUrl, value);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<T>();
        return default;
    }
}
