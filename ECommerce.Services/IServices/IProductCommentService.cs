namespace ECommerce.Services.IServices;

public interface IProductCommentService : IEntityService<ProductComment>
{
    Task<ServiceResult> Add(ProductComment productComment);

    Task<ServiceResult<List<ProductComment>>> GetAllAcceptedComments(string search = "", int pageNumber = 0,
        int pageSize = 10);

    Task<ServiceResult<List<ProductComment>>> Load(string search = "", int pageNumber = 0, int pageSize = 10);
    Task<ServiceResult<ProductComment>> GetById(int id);
    Task<ServiceResult> Edit(ProductComment productComment);
    Task<ServiceResult> Accept(ProductComment productComment);
    Task<ServiceResult> Delete(int id);
}