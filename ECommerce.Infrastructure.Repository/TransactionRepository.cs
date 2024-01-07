namespace ECommerce.Infrastructure.Repository;

public class TransactionRepository(SunflowerECommerceDbContext context) : AsyncRepository<Transaction>(context),
    ITransactionRepository
{
    public async Task<PagedList<Transaction>> Search(TransactionFiltreViewModel transactionFiltreViewModel,
        CancellationToken cancellationToken)
    {
        var query = context.Transactions
            .Where(x => x.User.UserName.Contains(transactionFiltreViewModel.PaginationParameters.Search))
            .Include(d => d.PaymentMethod)
            .Include(x => x.User)
            .AsNoTracking();

        if (transactionFiltreViewModel.UserId > 0)
            query = query.Where(x => x.UserId == transactionFiltreViewModel.UserId);
        if (transactionFiltreViewModel.ToTransactionDate != null)
            query = query.Where(x => x.TransactionDate <= transactionFiltreViewModel.ToTransactionDate);
        if (transactionFiltreViewModel.FromTransactionDate != null)
            query = query.Where(x => x.TransactionDate >= transactionFiltreViewModel.FromTransactionDate);
        if (transactionFiltreViewModel.MinimumAmount != null)
            query = query.Where(x => x.Amount >= transactionFiltreViewModel.MinimumAmount);
        if (transactionFiltreViewModel.MaximumAmount != null)
            query = query.Where(x => x.Amount <= transactionFiltreViewModel.MaximumAmount);
        if (transactionFiltreViewModel.PaymentMethodStatus != null)
            query = query.Where(x =>
                x.PaymentMethod.PaymentMethodStatus == transactionFiltreViewModel.PaymentMethodStatus);


        var sortedQuery = query.OrderByDescending(x => x.Id);
        switch (transactionFiltreViewModel.TransactionSort)
        {
            case TransactionSort.LowToHighPiceBuying:
                sortedQuery = query.OrderBy(x => x.Amount);
                break;
            case TransactionSort.HighToLowPriceBuying:
                sortedQuery = query.OrderByDescending(x => x.Amount);
                break;
            case TransactionSort.LowToHighDateBuying:
                sortedQuery = query.OrderBy(x => x.TransactionDate);
                break;
            case TransactionSort.HighToLowDateBuying:
                sortedQuery = query.OrderByDescending(x => x.TransactionDate);
                break;
        }

        var transactionList = await sortedQuery.Select(p => new Transaction
        {
            Id = p.Id,
            Amount = p.Amount,
            TransactionDate = p.TransactionDate,
            PaymentMethod = p.PaymentMethod,
            UserId = p.UserId
        }).ToListAsync(cancellationToken);

        return PagedList<Transaction>.ToPagedList(transactionList,
            transactionFiltreViewModel.PaginationParameters.PageNumber,
            transactionFiltreViewModel.PaginationParameters.PageSize);
    }
}
