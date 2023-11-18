using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Services.Services;

public class HolooMGroupService(IHttpService http) : EntityService<HolooMGroup, HolooMGroup, HolooMGroup>(http), IHolooMGroupService
{
    private const string Url = "api/Products";

    public async Task<ApiResult<List<HolooMGroup>>> Load()
    {
        return await ReadList(Url, "GetMGroup");
    }
}