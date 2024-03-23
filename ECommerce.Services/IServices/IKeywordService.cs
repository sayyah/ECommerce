using ECommerce.API.DataTransferObject.Keywords;

namespace ECommerce.Services.IServices;

public interface IKeywordService : IEntityService<ReadKeywordDto>
{
    Task<ServiceResult<List<ReadKeywordDto>>> GetKeywordsByProductId(int productId);
    Task<ServiceResult<List<ReadKeywordDto>>> GetAll();
    Task<ServiceResult<List<ReadKeywordDto>>> Filtering(string filter);
    Task<ServiceResult<List<ReadKeywordDto>>> Load(string search = "", int pageNumber = 0, int pageSize = 10);
    Task<ServiceResult<Dictionary<int, string>>> LoadDictionary();
    Task<ServiceResult> Add(ReadKeywordDto keyword);
    Task<ServiceResult> Edit(ReadKeywordDto keyword);
    Task<ServiceResult> Delete(int id);
    Task<ServiceResult<ReadKeywordDto>> GetById(int id);
}