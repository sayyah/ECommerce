﻿using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Tags;

public class CreateModel : PageModel
{
    private readonly ITagService _tagService;

    public CreateModel(ITagService tagService)
    {
        _tagService = tagService;
    }

    [BindProperty] public Tag Tag { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (Tag.TagText.Contains("@") || Tag.TagText.Contains("&") || Tag.TagText.Contains("*") ||
            Tag.TagText.Contains("/") || Tag.TagText.Contains("\\"))
        {
            Message = "از علامت های @ & * / \\ استفاده نکنید";
            Code = "Error";
            return Page();
        }

        if (ModelState.IsValid)
        {
            var result = await _tagService.Add(Tag);
            if (result.Code == 0)
                return RedirectToPage("/Tags/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }
}