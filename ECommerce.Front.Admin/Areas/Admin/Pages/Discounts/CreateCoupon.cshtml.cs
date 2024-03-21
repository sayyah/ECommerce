﻿using Ecommerce.Entities;
using Ecommerce.Entities.ViewModel;
using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace ECommerce.Front.BolouriGroup.Areas.Admin.Pages.Discounts;

public class CreateCouponModel : PageModel
{
    private readonly IDiscountService _discountService;
    private readonly IProductService _productService;
    private readonly IPriceService _priceService;
    private readonly ICategoryService _categoryService;    

    public CreateCouponModel(IDiscountService discountService, IProductService productService, IPriceService priceService
        ,ICategoryService categoryService)
    {
        _discountService = discountService;
        _productService = productService;
        _priceService = priceService;
        _categoryService = categoryService;
    }

    [BindProperty] public bool WithPrice { get; set; }
    [BindProperty] public DiscountViewModel Discount { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (WithPrice && !Discount.Amount.HasValue) ModelState.AddModelError(string.Empty, "مبلغ نباید خالی باشد.");
        if (!WithPrice && !Discount.Percent.HasValue) ModelState.AddModelError(string.Empty, "درصد نباید خالی باشد.");
        if (!Discount.StartDate.HasValue) ModelState.AddModelError(string.Empty, "تاریخ شروع نباید خالی باشد.");
        if (!Discount.EndDate.HasValue) ModelState.AddModelError(string.Empty, "تاریخ پایان نباید خالی باشد.");
        if (Discount.StartDate > Discount.EndDate) ModelState.AddModelError(string.Empty, "تاریخ پایان نباید قبل از تاریخ شروع باشد.");
        if (Discount.MinOrder > Discount.MaxOrder) ModelState.AddModelError(string.Empty, "حداقل تعداد سفارش باید کم تر از حداکثر آن باشد.");
        Discount.DiscountType = DiscountType.Coupon;
        if (ModelState.IsValid)
        {
            if (WithPrice) Discount.Percent = null;
            else Discount.Amount = null;
            var result = await _discountService.Add(Discount);
            if (result.Code == 0)
                return RedirectToPage("/Discounts/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }
}