using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Domain.Interfaces;

public interface IHolooCustomerRepository : IHolooRepository<HolooCustomer>
{
<<<<<<< HEAD
    Task<string> AddWithoutSave(HolooCustomer customer, CancellationToken cancellationToken);
    Task<(string customerCode, string customerCodeC)> GetNewCustomerCode();

    Task<HolooCustomer> GetCustomerByCode(string customerCode);
    Task SaveAddedCustomer (CancellationToken cancellationToken);
=======
    Task<string> Add(HolooCustomer customer, CancellationToken cancellationToken);
    Task<(string customerCode, string customerCodeC)> GetNewCustomerCode();

    Task<HolooCustomer> GetCustomerByCode(string customerCode);
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)
}
