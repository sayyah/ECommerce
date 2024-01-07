using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Pages;

public class BlogModel(IBlogService blogService, IBlogCategoryService blogCategoryService, ITagService tagService)
    : PageModel
{
    private readonly IBlogCategoryService _blogCategoryService = blogCategoryService;

    public ServiceResult<List<BlogViewModel>> Blogs { get; set; }
    [BindProperty] public ServiceResult<List<Tag>> Tags { get; set; }
    public string? Search { get; set; }

    public async Task OnGet(string blogCategoryId, string search, int pageNumber = 1, int pageSize = 3,
        int productSort = 1,
        string message = null, string code = null)
    {
        if (int.TryParse(blogCategoryId, out var intResult))
            Blogs = await blogService.TopBlogs(blogCategoryId, search, pageNumber, pageSize);
        else
            Blogs = await blogService.TopBlogsByTagText(null, blogCategoryId, pageNumber, pageSize);

        Tags = await tagService.GetAllBlogTags();
    }

    public async Task OnPost(string blogCategoryId, string search)
    {
        Blogs = await blogService.TopBlogs(blogCategoryId, search, 1, 3);
        Tags = await tagService.GetAllBlogTags();
    }
}