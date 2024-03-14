using ECommerce.API.DataTransferObject.BlogComments.Queries;

namespace ECommerce.Services.IServices;

public interface IBlogCommentService : IEntityService<ReadBlogCommentDto>
{
    Task<ServiceResult<List<ReadBlogCommentDto>>> Filtering(string filter);
    Task<ServiceResult<List<ReadBlogCommentDto>>> Load(string search = "", int pageNumber = 0, int pageSize = 10);
    Task<ServiceResult<Dictionary<int, string>>> LoadDictionary();
    Task<ServiceResult> Add(ReadBlogCommentDto blogComment);
    Task<ServiceResult> Edit(ReadBlogCommentDto blogComment);
    Task<ServiceResult> Accept(ReadBlogCommentDto blogComment);
    Task<ServiceResult> Delete(int id);
    Task<ServiceResult<ReadBlogCommentDto>> GetById(int id);

    Task<ServiceResult<List<ReadBlogCommentDto>>> GetAllAcceptedComments(string search = "", int pageNumber = 0,
        int pageSize = 10);
}