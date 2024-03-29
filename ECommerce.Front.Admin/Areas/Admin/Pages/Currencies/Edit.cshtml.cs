﻿using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Currencies;

public class EditModel(ICurrencyService currencyService) : PageModel
{
    [BindProperty] public Currency Currency { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }

    public async Task OnGet(int id)
    {
        var result = await currencyService.GetById(id);
        Currency = result.ReturnData;
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var result = await currencyService.Edit(Currency);
            Message = result.Message;
            Code = result.Code.ToString();
            if (result.Code == 0)
                return RedirectToPage("/Currencies/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }
}