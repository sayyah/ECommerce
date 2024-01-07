using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Blogs;

public class CreateModel(IBlogService blogService, IImageService imageService, ITagService tagService,
        IKeywordService keywordService, IHostEnvironment environment, IBlogAuthorService blogAuthorService,
        IBlogCategoryService blogCategoryService)
    : PageModel
{
    [BindProperty] public BlogViewModel Blog { get; set; }

    [BindProperty] public IFormFile? Upload { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }
    public SelectList Tags { get; set; }
    public SelectList Keywords { get; set; }
    public SelectList BlogAuthors { get; set; }
    public List<CategoryParentViewModel> Categories { get; set; }

    private async Task Initial()
    {
        Blog = new BlogViewModel();

        var tags = (await tagService.GetAll()).ReturnData;
        Tags = new SelectList(tags, nameof(Tag.Id), nameof(Tag.TagText));

        var keywords = (await keywordService.GetAll()).ReturnData;
        Keywords = new SelectList(keywords, nameof(Keyword.Id), nameof(Keyword.KeywordText));

        var blogAuthors = (await blogAuthorService.GetAll()).ReturnData;
        BlogAuthors = new SelectList(blogAuthors, nameof(BlogAuthor.Id), nameof(BlogAuthor.Name));

        var resultParent = await blogCategoryService.GetParents();
        Categories = resultParent.ReturnData;
    }

    public async Task OnGet()
    {
        await Initial();
    }

    public async Task<IActionResult> OnPost()
    {
        if (Blog.BlogCategoryId == 0)
        {
            Message = "لطفا دسته بندی را انتخاب کنید";
            Code = ServiceCode.Error.ToString();
            await Initial();
            return Page();
        }

        if (Upload == null)
        {
            Message = "لطفا عکس را انتخاب کنید";
            Code = ServiceCode.Error.ToString();
            await Initial();
            return Page();
        }

        if (Upload.FileName.Split('.').Last().ToLower() != "webp")
        {
            ModelState.AddModelError("IvalidFileExtention", "فرمت فایل پشتیبانی نمی‌شود.");
            await Initial();
            return Page();
        }


        if (ModelState.IsValid)
        {
            var result = await blogService.Add(Blog);
            if (result.Code == 0)
            {
                var resultImage = await imageService.Add(Upload, result.ReturnData.Id, "Images/Blogs",
                    environment.ContentRootPath);

                if (resultImage.Code == 0)
                    return RedirectToPage("/Blogs/Index",
                        new { area = "Admin", message = result.Message, code = result.Code.ToString() });
                Message = result.Message;
                Code = result.Code.ToString();
                ModelState.AddModelError("", result.Message);
            }
            else
            {
                Message = result.Message;
                Code = result.Code.ToString();
            }
        }

        await Initial();
        return Page();
    }
}