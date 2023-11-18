using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Services.Services;

public class HolooUnitService : EntityService<HolooUnit, HolooUnit, HolooUnit>, IHolooUnitService
{
    private const string Url = "api/Units";

    public HolooUnitService(IHttpService http) : base(http)
    {
    }

    public async Task<ServiceResult<List<HolooUnit>>> Load()
    {
        var result = await ReadList(Url, "GetHolooUnits");
        return Return(result);
    }
}