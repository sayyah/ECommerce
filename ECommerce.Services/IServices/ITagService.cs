using ECommerce.API.DataTransferObject.Tags;
using ECommerce.Application.ViewModels;

namespace ECommerce.Services.IServices;

public interface ITagService : IEntityService<ReadTagDto>
{
    Task<ServiceResult<List<ReadTagDto>>> GetAll();
    Task<ServiceResult<List<ReadTagDto>>> Filtering(string filter);
    Task<ServiceResult<ReadTagDto>> GetByTagText(string tagText);
    Task<ServiceResult<List<ReadTagDto>>> Load(string search = "", int pageNumber = 0, int pageSize = 10);
    Task<ServiceResult<Dictionary<int, string>>> LoadDictionary();
    Task<ServiceResult> Add(ReadTagDto tag);
    Task<ServiceResult> Edit(ReadTagDto tag);
    Task<ServiceResult> Delete(int id);
    Task<ServiceResult<ReadTagDto>> GetById(int id);
    Task<ServiceResult<List<TagProductId>>> GetTagsByProductId(int productId);
    Task<ServiceResult<List<ReadTagDto>>> GetAllProductTags();
    Task<ServiceResult<List<ReadTagDto>>> GetAllBlogTags();
}