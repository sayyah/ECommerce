using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IPaymentMethodRepository : IAsyncRepository<PaymentMethod>
{
    Task<PagedList<PaymentMethod>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);

    Task<PaymentMethod> GetByAccountNumber(string name, CancellationToken cancellationToken);

    Task<int> AddAll(IEnumerable<PaymentMethod> paymentMethods, CancellationToken cancellationToken);
}
