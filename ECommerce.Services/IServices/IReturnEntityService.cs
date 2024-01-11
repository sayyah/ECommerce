
namespace ECommerce.Services.IServices
{
    public interface IReturnEntityService
    {
        ServiceResult Return(ApiResult result);
        ServiceResult<TResult> Return<TResult>(ApiResult<TResult> result);
        ServiceResult Return(ApiResult<object> result);
    }
}
