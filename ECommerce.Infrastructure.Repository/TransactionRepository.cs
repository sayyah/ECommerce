namespace ECommerce.Infrastructure.Repository;

public class TransactionRepository(SunflowerECommerceDbContext context) : RepositoryBase<Transaction>(context),
    ITransactionRepository
{
    public async Task<PagedList<Transaction>> Search(transactionFilterViewModel transactionFilterViewModel,
        CancellationToken cancellationToken)
    {
        var query = context.Transactions
            .Where(x => x.User.UserName.Contains(transactionFilterViewModel.PaginationParameters.Search))
            .Include(d => d.PaymentMethod)
            .Include(x => x.User)
            .AsNoTracking();

        if (transactionFilterViewModel.UserId > 0)
            query = query.Where(x => x.UserId == transactionFilterViewModel.UserId);
        if (transactionFilterViewModel.ToTransactionDate != null)
            query = query.Where(x => x.TransactionDate <= transactionFilterViewModel.ToTransactionDate);
        if (transactionFilterViewModel.FromTransactionDate != null)
            query = query.Where(x => x.TransactionDate >= transactionFilterViewModel.FromTransactionDate);
        if (transactionFilterViewModel.MinimumAmount != null)
            query = query.Where(x => x.Amount >= transactionFilterViewModel.MinimumAmount);
        if (transactionFilterViewModel.MaximumAmount != null)
            query = query.Where(x => x.Amount <= transactionFilterViewModel.MaximumAmount);
        if (transactionFilterViewModel.PaymentMethodStatus != null)
            query = query.Where(x =>
                x.PaymentMethod.PaymentMethodStatus == transactionFilterViewModel.PaymentMethodStatus);


        var sortedQuery = query.OrderByDescending(x => x.Id);
        switch (transactionFilterViewModel.TransactionSort)
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
            transactionFilterViewModel.PaginationParameters.PageNumber,
            transactionFilterViewModel.PaginationParameters.PageSize);
    }
}
