namespace ECommerce.Services.IServices;

public interface IBlogService : IEntityService<Blog>
{
    Task<ServiceResult<List<Blog>>> Filtering(string filter);
    Task<ServiceResult<List<Blog>>> Load(string search = "", int pageNumber = 0, int pageSize = 10);
    Task<ServiceResult<Dictionary<int, string>>> LoadDictionary();
    Task<ServiceResult<Blog>> Add(Blog blog);
    Task<ServiceResult> Edit(Blog blog);
    Task<ServiceResult> Delete(int id);
    Task<ServiceResult<Blog>> GetById(int id);

    Task<ServiceResult<List<Blog>>> TopBlogs(string CategoryId = "", string search = "",
        int pageNumber = 0, int pageSize = 10, int blogSort = 1);

    Task<ServiceResult<List<Blog>>> TopBlogsByTagText(string CategoryId = "", string TagText = "",
        int pageNumber = 0, int pageSize = 10, int blogSort = 1);

    Task<ServiceResult<Blog>> GetByUrl(string blogUrl);
}