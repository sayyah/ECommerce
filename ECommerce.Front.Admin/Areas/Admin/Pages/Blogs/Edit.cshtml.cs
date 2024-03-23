using ECommerce.API.DataTransferObject.Blogs.Queries;
using ECommerce.API.DataTransferObject.Keywords;
using ECommerce.API.DataTransferObject.Tags;
using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Blogs;

public class EditModel(IBlogService blogService, IImageService imageService, ITagService tagService,
        IKeywordService keywordService, IHostEnvironment environment, IBlogAuthorService blogAuthorService,
        IBlogCategoryService blogCategoryService)
    : PageModel
{
    [BindProperty] public ReadBlogDto? Blog { get; set; }
    [BindProperty] public IFormFile? Upload { get; set; }
    [TempData] public string? Message { get; set; }
    [TempData] public string? Code { get; set; }
    [TempData] public Image? BlogImage { get; set; }
    public List<ReadTagDto>? Tags { get; set; }
    public List<ReadKeywordDto>? Keywords { get; set; }
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
        if (Blog is { BlogCategoryId: 0 })
        {
            Message = "لطفا دسته بندی را انتخاب کنید";
            Code = ServiceCode.Error.ToString();
            await Initial(Blog.Id);
            return Page();
        }

        if (Blog != null)
        {
            var image = await imageService.GetImagesByBlogId(Blog.Id);
            BlogImage = image.ReturnData;

            if (Upload == null && BlogImage.Id == 0)
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


            if (Upload != null && BlogImage.Id != 0)
            {
                await imageService.Delete($"Images/Blogs/{BlogImage?.Name}", BlogImage.Id, environment.ContentRootPath);
                image = await imageService.GetImagesByBlogId(Blog.Id);
                BlogImage = image.ReturnData;
                Blog.ImageId = BlogImage.Id;
                Blog.ImagePath = BlogImage.Path;
                Blog.ImageName = BlogImage.Name;
                Blog.ImageAlt = BlogImage.Alt;
            }

            if (Upload != null && BlogImage.Id == 0)
            {
                var resultImage = await imageService.Add(Upload, Blog.Id, "Images/Blogs",
                    environment.ContentRootPath);
                if (resultImage.Code > 0)
                {
                    Message = resultImage.Message;
                    Code = resultImage.Code.ToString();
                    if (resultImage.Message != null) ModelState.AddModelError("", resultImage.Message);
                }

                image = await imageService.GetImagesByBlogId(Blog.Id);
                BlogImage = image.ReturnData;
                Blog.ImageId = BlogImage.Id;
                Blog.ImagePath = BlogImage.Path;
                Blog.ImageName = BlogImage.Name;
                Blog.ImageAlt = BlogImage.Alt;
            }
        }


        if (ModelState.IsValid)
        {
            if (Blog != null)
            {
                var result = await blogService.Edit(Blog);
                if (result.Code == 0)
                    return RedirectToPage("/Blogs/Index",
                        new { area = "Admin", message = result.Message, code = result.Code.ToString() });

                Message = result.Message;
                Code = result.Code.ToString();
                if (result.Message != null) ModelState.AddModelError("", result.Message);
            }
        }

        if (Blog != null) await Initial(Blog.Id);
        return Page();
    }

    private async Task<ServiceResult<ReadBlogDto>> Initial(int id)
    {
        var result = await blogService.GetById(id);
        if (result.Code > 0)
            return new ServiceResult<ReadBlogDto>
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
        if (result.Message != null) ModelState.AddModelError("", result.Message);

        await Initial(blogId);
        return Page();
    }
}