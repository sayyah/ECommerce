using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ECommerce.Services.IServices;

namespace Bolouri.Areas.Admin.Pages.Blogs;

public class DeleteModel : PageModel
{
    private readonly IBlogService _blogService;

    public DeleteModel(IBlogService blogService)
    {
        _blogService = blogService;
    }

    public Blog Blog { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await _blogService.GetById(id);
        if (result.Code == 0)
        {
            Blog = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Blogs/Index",
            new {area = "Admin", message = result.Message, code = result.Code.ToString()});
    }

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await _blogService.Delete(id);
            Message = result.Message;
            Code = result.Code.ToString();
            if (result.Code == 0)
                return RedirectToPage("/Blogs/Index",
                    new {area = "Admin", message = result.Message, code = result.Code.ToString()});
            Message = result.Message;
            Code = result.Code.ToString();
        }

        return Page();
    }
}