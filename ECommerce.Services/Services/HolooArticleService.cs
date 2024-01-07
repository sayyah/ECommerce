using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Services.Services;

public class HolooArticleService(IHttpService http) : EntityService<HolooArticle>(http), IHolooArticleService
{
    private const string Url = "api/Products";

    public async Task<ServiceResult<List<HolooArticle>>> Load(string code)
    {
        var result = await ReadList(Url, $"GetAllArticleMCodeSCode?code={code}");
        if (result.Code == ResultCode.Success)
            return new ServiceResult<List<HolooArticle>>
            {
                Code = ServiceCode.Success,
                ReturnData = result.ReturnData,
                Message = result.Messages?.FirstOrDefault()
            };
        return new ServiceResult<List<HolooArticle>>
        {
            Code = ServiceCode.Error,
            Message = result.GetBody()
        };
    }
}