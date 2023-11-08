using Ecommerce.Entities.Helper;

namespace ECommerce.Services.IServices
{
    public interface IReadEntityService<TRead>
    {
        Task<ApiResult<List<TRead>>> ReadList(string url);
        Task<ApiResult<List<TRead>>> ReadList(string url, string api);
        Task<ApiResult<TRead>> Read(string url);
        Task<ApiResult<TRead>> Read(string url, string api);
    }
}
