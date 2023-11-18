using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Services.IServices;

public interface IHolooAccountNumberService : IEntityService<HolooAccountNumber, HolooAccountNumber, HolooAccountNumber>
{
    List<HolooAccountNumber> HolooAccountNumbers { get; set; }
    Task Load();
}