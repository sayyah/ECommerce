using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.PaymentMethods;

public class DetailModel(IPaymentMethodService paymentMethodService) : PageModel
{
    public PaymentMethod PaymentMethod { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await paymentMethodService.GetById(id);
        if (result.Code == 0)
        {
            PaymentMethod = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/PaymentMethods/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}