using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IPaymentMethodRepository : IRepositoryBase<PaymentMethod>
{
    PagedList<PaymentMethod> Search(PaginationParameters paginationParameters);

    Task<PaymentMethod?> GetByAccountNumber(string name, CancellationToken cancellationToken);

    void AddAll(IEnumerable<PaymentMethod> paymentMethods);
}
