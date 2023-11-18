using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Services.IServices;

public interface IHolooUnitService : IEntityService<HolooUnit, HolooUnit, HolooUnit>
{
    Task<ServiceResult<List<HolooUnit>>> Load();
}