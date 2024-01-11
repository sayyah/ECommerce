using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Blogs;

public class EditModel(IBlogService blogService, IImageService imageService, ITagService tagService,
        IKeywordService keywordService, IHostEnvironment environment, IBlogAuthorService blogAuthorService,
        IBlogCategoryService blogCategoryService)
    : PageModel
{
    [BindProperty] public Blog Blog { get; set; }
    [BindProperty] public IFormFile? Upload { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }
    public List<Tag> Tags { get; set; }
    public List<Keyword> Keywords { get; set; }
    public List<BlogAuthor> BlogAuthors { get; set; }
    public List<BlogCategory> Categories { get; set; }


    public async Task<IActionResult> OnGet(int id)
    {
        var result = await Initial(id);
        if (result.Code == 0) return Page();
        return RedirectToPage("/Blogs/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost()
    {
        if (Blog.BlogCategoryId == 0)
        {
            Message = "لطفا دسته بندی را انتخاب کنید";
            Code = ServiceCode.Error.ToString();
            await Initial(Blog.Id);
            return Page();
        }

        var _image = await imageService.GetImagesByBlogId(Blog.Id);
        Blog.Image = _image.ReturnData;

        if (Upload == null && Blog.Image.Id == 0)
        {
            Message = "لطفا عکس را انتخاب کنید";
            Code = ServiceCode.Error.ToString();
            await Initial(Blog.Id);
            return Page();
        }

        if (Upload != null)
            if (Upload.FileName.Split('.').Last().ToLower() != "webp")
            {
                ModelState.AddModelError("IvalidFileExtention", "فرمت فایل پشتیبانی نمی‌شود.");
                await Initial(Blog.Id);
                return Page();
            }


        if (Upload != null && Blog.Image.Id != 0)
        {
            await imageService.Delete($"Images/Blogs/{Blog.Image?.Name}", Blog.Image.Id, environment.ContentRootPath);
            _image = await imageService.GetImagesByBlogId(Blog.Id);
            Blog.Image = _image.ReturnData;
        }

        if (Upload != null && Blog.Image.Id == 0)
        {
            var resultImage = await imageService.Add(Upload, Blog.Id, "Images/Blogs",
                environment.ContentRootPath);
            if (resultImage.Code > 0)
            {
                Message = resultImage.Message;
                Code = resultImage.Code.ToString();
                ModelState.AddModelError("", resultImage.Message);
            }

            _image = await imageService.GetImagesByBlogId(Blog.Id);
            Blog.Image = _image.ReturnData;
        }


        if (ModelState.IsValid)
        {
            var result = await blogService.Edit(Blog);
            if (result.Code == 0)
                return RedirectToPage("/Blogs/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });

            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        await Initial(Blog.Id);
        return Page();
    }

    private async Task<ServiceResult<Blog>> Initial(int id)
    {
        var result = await blogService.GetById(id);
        if (result.Code > 0)
            return new ServiceResult<Blog>
            {
                Code = result.Code,
                Message = result.Message
            };
        Blog = result.ReturnData;

        Tags = (await tagService.GetAll()).ReturnData;
        Keywords = (await keywordService.GetAll()).ReturnData;
        Categories = (await blogCategoryService.GetAll()).ReturnData;
        BlogAuthors = (await blogAuthorService.GetAll()).ReturnData;
        //BlogAuthors = new SelectList(blogAuthors, nameof(BlogAuthor.Id), nameof(BlogAuthor.Name));


        return result;
    }

    public async Task<IActionResult> OnPostDeleteImage(string imageName, int id, int blogId)
    {
        var result = await imageService.Delete($"Images/Blogs/{imageName}", id, environment.ContentRootPath);

        if (result.Code == 0)
            //return RedirectToPage("/Blogs/Edit",
            //    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
        Code = result.Code.ToString();
        ModelState.AddModelError("", result.Message);

        await Initial(blogId);
        return Page();
    }
}