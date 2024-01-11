using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IPaymentMethodRepository : IRepositoryBase<PaymentMethod>
{
    Task<PagedList<PaymentMethod>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);

    Task<PaymentMethod?> GetByAccountNumber(string name, CancellationToken cancellationToken);

    void AddAll(IEnumerable<PaymentMethod> paymentMethods);
}
