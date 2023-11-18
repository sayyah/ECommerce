using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Services.IServices;

public interface IHolooMGroupService : IEntityService<HolooMGroup, HolooMGroup, HolooMGroup>
{
    Task<ApiResult<List<HolooMGroup>>> Load();
}