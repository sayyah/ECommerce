using ECommerce.API.DataTransferObject.Blogs.Queries;

namespace ECommerce.Services.IServices;

public interface IBlogService : IEntityService<ReadBlogDto>
{
    Task<ServiceResult<List<ReadBlogDto>>> Filtering(string filter);
    Task<ServiceResult<List<ReadBlogDto>>> Load(string search = "", int pageNumber = 0, int pageSize = 10);
    Task<ServiceResult<Dictionary<int, string>>> LoadDictionary();
    Task<ServiceResult<ReadBlogDto>> Add(ReadBlogDto blog);
    Task<ServiceResult> Edit(ReadBlogDto blog);
    Task<ServiceResult> Delete(int id);
    Task<ServiceResult<ReadBlogDto>> GetById(int id);

    Task<ServiceResult<List<ReadBlogDto>>> TopBlogs(string CategoryId = "", string search = "",
        int pageNumber = 0, int pageSize = 10, int blogSort = 1);

    Task<ServiceResult<List<ReadBlogDto>>> TopBlogsByTagText(string CategoryId = "", string TagText = "",
        int pageNumber = 0, int pageSize = 10, int blogSort = 1);

    Task<ServiceResult<ReadBlogDto>> GetByUrl(string blogUrl);
}