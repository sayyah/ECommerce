using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Domain.Interfaces;

public interface IHolooCustomerRepository : IHolooRepository<HolooCustomer>
{
    Task<string> Add(HolooCustomer customer, CancellationToken cancellationToken);
    Task<(string customerCode, string customerCodeC)> GetNewCustomerCode();

    Task<HolooCustomer> GetCustomerByCode(string customerCode);
}
