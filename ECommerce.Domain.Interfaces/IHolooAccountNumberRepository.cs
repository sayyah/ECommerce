using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Domain.Interfaces;

public interface IHolooAccountNumberRepository : IHolooRepository<HolooAccountNumber>
{
    Task<HolooAccountNumber?> GetByAccountNumberAndBankCode(string code, CancellationToken cancellationToken);
}
