using Ecommerce.Entities.Helper;

namespace ECommerce.Services.IServices
{
    public interface IDeleteEntityService
    {
        Task<ApiResult> Delete(string url, int id);
    }
}
