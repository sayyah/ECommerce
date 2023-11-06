﻿using ECommerce.Services.IServices;

<<<<<<< HEAD:ECommerce.Front.Admin/Areas/Admin/Pages/BlogComments/Edit.cshtml.cs
namespace ECommerce.Front.Admin.Areas.Admin.Pages.BlogComments;
=======
namespace ECommerce.Front.BolouriGroup.Areas.Admin.Pages.BlogComments;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8):ECommerce.Front.BolouriGroup/Areas/Admin/Pages/BlogComments/Edit.cshtml.cs

public class EditModel : PageModel
{
    private readonly IBlogCommentService _blogCommentService;
    private readonly IBlogService _blogService;

    public EditModel(IBlogCommentService blogCommentService, IBlogService blogService)
    {
        _blogCommentService = blogCommentService;
        _blogService = blogService;
    }

    [BindProperty] public BlogComment BlogComment { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }

    public async Task OnGet(int id, string message = null, string code = null)
    {
        Message = message;
        Code = code;
        var BlogCommentResult = await _blogCommentService.GetById(id);
        BlogComment = BlogCommentResult.ReturnData;
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            if (BlogComment.Answer!.Text == null && BlogComment.AnswerId == null) BlogComment.Answer = null;
            var result = await _blogCommentService.Edit(BlogComment);
            Message = result.Message;
            Code = result.Code.ToString();
            if (result.Code == 0)
                return RedirectToPage("/BlogComments/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
            return RedirectToPage("/BlogComments/Edit",
                new { id = BlogComment.Id, area = "Admin", message = $"پیغام خطا:{Message}", code = Code });
        }
        catch (Exception ex)
        {
            return RedirectToPage("/BlogComments/Edit",
                new { id = BlogComment.Id, area = "Admin", message = "پیغام خطای غیر منتظره", code = "Error" });
        }
    }
}
 
