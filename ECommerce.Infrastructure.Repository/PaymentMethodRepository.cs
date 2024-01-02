﻿using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class PaymentMethodRepository : RepositoryBase<PaymentMethod>, IPaymentMethodRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public PaymentMethodRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PaymentMethod?> GetByAccountNumber(string name, CancellationToken cancellationToken)
    {
        return await _context.PaymentMethods.Where(x => x.AccountNumber == name).FirstOrDefaultAsync();
    }

    public void AddAll(IEnumerable<PaymentMethod> paymentMethods)
    {
        _context.PaymentMethods.AddRangeAsync(paymentMethods);
    }

    public async Task<PagedList<PaymentMethod>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        if (!paginationParameters.Search.Equals(""))
            return PagedList<PaymentMethod>.ToPagedList(
                await _context.PaymentMethods
                    .Where(x => x.Transactions.Any(r => r.RefId.Contains(paginationParameters.Search))).AsNoTracking()
                    .OrderBy(on => on.Id).ToListAsync(cancellationToken),
                paginationParameters.PageNumber,
                paginationParameters.PageSize);

        return PagedList<PaymentMethod>.ToPagedList(
            await _context.PaymentMethods.AsNoTracking().OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
