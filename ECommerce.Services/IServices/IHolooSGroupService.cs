using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Services.IServices;

public interface IHolooSGroupService : IEntityService<HolooSGroup, HolooSGroup, HolooSGroup>
{
    Task<ServiceResult<List<HolooSGroup>>> Load(string mGroupCode);
}