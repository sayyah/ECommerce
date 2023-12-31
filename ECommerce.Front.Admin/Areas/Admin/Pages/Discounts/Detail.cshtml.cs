using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Discounts;

public class DetailModel : PageModel
{
    private readonly IDiscountService _discountService;

    public DetailModel(IDiscountService discountService)
    {
        _discountService = discountService;
    }

    public Discount Discount { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await _discountService.GetById(id);
        if (result.Code == 0)
        {
            Discount = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Discounts/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}