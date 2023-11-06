﻿using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Areas.Admin.Pages.BlogCategories;

public class CreateModel : PageModel
{
    private readonly IBlogCategoryService _blogCategoryService;

    public CreateModel(IBlogCategoryService blogCategoryService)
    {
        _blogCategoryService = blogCategoryService;
    }

    [BindProperty] public BlogCategory BlogCategory { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var result = await _blogCategoryService.Add(BlogCategory);
            if (result.Code == 0)
                return RedirectToPage("/BlogCategories/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }
}