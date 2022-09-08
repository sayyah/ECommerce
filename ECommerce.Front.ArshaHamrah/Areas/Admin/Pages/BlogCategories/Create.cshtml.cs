using Entities;
using Entities.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Entities.ViewModel;

namespace ArshaHamrah.Areas.Admin.Pages.BlogCategories;

[Authorize(Roles = "Admin,SuperAdmin")]
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
    public List<CategoryParentViewModel> BlogCategories { get; set; }

    public async void OnGet()
    {
        var result = await _blogCategoryService.GetParents();
        BlogCategories = result.ReturnData;
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var result = await _blogCategoryService.Add(BlogCategory);
            if (result.Code == 0)
                return RedirectToPage("/BlogCategories/Index",
                    new {area = "Admin", message = result.Message, code = result.Code.ToString()});
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }
        var resultParent = await _blogCategoryService.GetParents();
        BlogCategories = resultParent.ReturnData;
        return Page();
    }
}