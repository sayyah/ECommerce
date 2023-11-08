using Ecommerce.Entities.Helper;

namespace ECommerce.Services.IServices
{
    public interface IUpdateEntityService<in TUpdate>
    {
        Task<ApiResult> Update(string url, TUpdate entity);
        Task<ApiResult> Update(string url, TUpdate entity, string apiName);
        Task<ApiResult> UpdateWithReturnId(string url, TUpdate entity);
    }
}
