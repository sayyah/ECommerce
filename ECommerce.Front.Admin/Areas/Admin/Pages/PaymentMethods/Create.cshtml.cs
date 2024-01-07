using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.PaymentMethods;

public class CreateModel(IPaymentMethodService paymentMethodService) : PageModel
{
    [BindProperty] public PaymentMethod PaymentMethod { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var result = await paymentMethodService.Add(PaymentMethod);
            if (result.Code == 0)
                return RedirectToPage("/PaymentMethods/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }
}