using Ecommerce.Entities.ViewModel;
using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Discounts;

public class CreateModel(IDiscountService discountService, ICategoryService categoryService) : PageModel
{
 

    [BindProperty] public bool WithPrice { get; set; }
    [BindProperty] public DiscountViewModel Discount { get; set; }

    [BindProperty] public List<CategoryParentViewModel> CategoryParentViewModel { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task OnGet()
    {
        CategoryParentViewModel = (await categoryService.GetParents()).ReturnData;
    }

    public async Task<IActionResult> OnPost()
    {
        if (WithPrice && !Discount.Amount.HasValue) ModelState.AddModelError(string.Empty, "مبلغ نباید خالی باشد.");
        if (!WithPrice && !Discount.Percent.HasValue) ModelState.AddModelError(string.Empty, "درصد نباید خالی باشد.");
        if (!Discount.StartDate.HasValue) ModelState.AddModelError(string.Empty, "تاریخ شروع نباید خالی باشد.");
        if (!Discount.EndDate.HasValue) ModelState.AddModelError(string.Empty, "تاریخ پایان نباید خالی باشد.");
        if (Discount.StartDate > Discount.EndDate)
            ModelState.AddModelError(string.Empty, "تاریخ پایان نباید قبل از تاریخ شروع باشد.");
        if (Discount.MinOrder > Discount.MaxOrder)
            ModelState.AddModelError(string.Empty, "حداقل تعداد سفارش باید کم تر از حداکثر آن باشد.");
        Discount.DiscountType = DiscountType.Category;
        if (ModelState.IsValid)
        {
            if (WithPrice) Discount.Percent = null;
            else Discount.Amount = null;
            var result = await discountService.Add(Discount);
            if (result.Code == 0)
                return RedirectToPage("/Discounts/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }
        
        CategoryParentViewModel = (await categoryService.GetParents()).ReturnData;
        if (Discount.CategoriesId==null) Discount.CategoriesId?.Clear();

        return Page();
    }
}