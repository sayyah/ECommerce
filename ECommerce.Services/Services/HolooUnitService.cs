using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Services.Services;

public class HolooUnitService(IHttpService http) : EntityService<HolooUnit>(http), IHolooUnitService
{
    private const string Url = "api/Units";

    public async Task<ServiceResult<List<HolooUnit>>> Load()
    {
        var result = await ReadList(Url, "GetHolooUnits");
        return Return(result);
    }
}