using Ecommerce.Entities.Helper;

namespace ECommerce.Services.IServices
{
    public interface ICreateEntityService<in TCreate>
    {
        Task<ApiResult<object>> Create(string url, TCreate entity);
        Task<ApiResult<TResponse>> Create<TResponse>(string url, TCreate entity);
    }
}
