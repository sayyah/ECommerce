using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IDiscountRepository : IRepositoryBase<Discount>
{
    Task<PagedList<Discount>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);

    Task<Discount?> GetByName(string name, CancellationToken cancellationToken);

    Task<Discount?> GetByCode(string code, CancellationToken cancellationToken);

    Task<Discount?> GetLast(CancellationToken cancellationToken);

    Task<DiscountWithTimeViewModel?> GetWithTime(CancellationToken cancellationToken);

    bool Active(int id);
}
