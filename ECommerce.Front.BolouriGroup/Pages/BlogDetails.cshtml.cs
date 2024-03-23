using ECommerce.API.DataTransferObject.Blogs.Queries;
using ECommerce.API.DataTransferObject.Tags;
using ECommerce.Application.Services.Blogs.Results;
using ECommerce.Front.BolouriGroup.Models;
using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Pages;

public class BlogDetailsModel(IBlogService blogService, IBlogCategoryService blogCategoryService,
        IBlogCommentService blogCommentService, IUserService userService, ITagService tagService)
    : PageModel
{
    public ReadBlogDto Blog { get; set; }
    public BlogCategory BlogCategory { get; set; }
    public ServiceResult<List<ReadBlogDto>> Blogs { get; set; }
    public ServiceResult<List<BlogCategory>> Categories { get; set; }
    public ServiceResult<List<BlogComment>> BlogComments { get; set; }
    public BlogComment? BlogComment { get; set; }
    [BindProperty] public string? Message { get; set; }
    [BindProperty] public ServiceResult<List<ReadTagDto>> Tags { get; set; }

    private async Task Initial(string blogUrl, int pageNumber = 1, int pageSize = 10)
    {
        var result = await blogService.GetByUrl(blogUrl);
        if (result.Code > 0) return;
        Blog = result.ReturnData;
        var blogCategory = await blogCategoryService.GetById(Blog.BlogCategoryId);
        BlogCategory = blogCategory.ReturnData;

        BlogComments =
            await blogCommentService.GetAllAcceptedComments(Convert.ToString(Blog.Id), pageNumber, pageSize);

        Blogs = await blogService.TopBlogs(null, null, 1, 3);
        Categories = await blogCategoryService.GetAll();

        Tags = await tagService.GetAllBlogTags();
    }

    public async Task OnGet(string blogUrl, int pageNumber = 1, int pageSize = 10)
    {
        await Initial(blogUrl, pageNumber, pageSize);
    }

    public async Task<IActionResult> OnGetComment(string blogUrl, string name, string email, string text)
    {
        VerifyResultData resultData = new();
        if (string.IsNullOrEmpty(name))
        {
            resultData.Description = "لطفا نام خود را برای ثبت نظر وارد کنید";
            resultData.Succeed = false;
            return new JsonResult(resultData);
        }

        if (string.IsNullOrEmpty(email))
        {
            resultData.Description = "لطفا ایمیل خود را برای ثبت نظر وارد کنید";
            resultData.Succeed = false;
            return new JsonResult(resultData);
        }

        if (string.IsNullOrEmpty(text))
        {
            resultData.Description = "لطفا نظر خود را برای ثبت نظر وارد کنید";
            resultData.Succeed = false;
            return new JsonResult(resultData);
        }

        BlogComment blogComment = new()
        {
            Email = email,
            Name = name,
            Text = text,
            User = null,
            UserId = null
        };

        var user = await userService.GetUser();
        if (user.Code == 0) blogComment.UserId = user.ReturnData.Id;

        var resultProduct = await blogService.GetByUrl(blogUrl);
        if (resultProduct.Code > 0) return new JsonResult(resultData);

        Blog = resultProduct.ReturnData;
        blogComment.BlogId = Blog.Id;

        var result = await blogCommentService.Add(blogComment);
        if (result.Code == 0)
        {
            resultData.Description = "نظر شما ثبت شد، پس از تایید توسط ادمین سایت، نمایش داده می شود";
            resultData.Succeed = true;
        }
        else
        {
            resultData.Description = "ثبت نظر با مشکل مواجه شد. لطفا مجددا تست کنید";
            resultData.Succeed = false;
        }

        return new JsonResult(resultData);
    }
}