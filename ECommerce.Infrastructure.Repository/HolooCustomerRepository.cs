using ECommerce.Domain.Entities.HolooEntity;
using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class HolooCustomerRepository : HolooRepository<HolooCustomer>, IHolooCustomerRepository
{
    private readonly HolooDbContext _context;

    public HolooCustomerRepository(HolooDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(string customerCode, string customerCodeC)> GetNewCustomerCode()
    {
        var customer = await _context.Customer.OrderByDescending(x => x.C_Code).FirstOrDefaultAsync();
        var customerCode = customer == null ? "00000" : customer.C_Code;
        var customerCodeC = customer == null ? "00000" : customer.C_Code_C;
        return ((Convert.ToInt32(customerCode) + 1).ToString("D5"), (Convert.ToInt32(customerCodeC) + 1).ToString());
    }

    public async Task<string> AddWithoutSave(HolooCustomer customer, CancellationToken cancellationToken)
    {
        await _context.Customer.AddAsync(customer, cancellationToken);     
        return customer.C_Code;
    }

    public async Task<HolooCustomer> GetCustomerByCode(string customerCode)
    {
        return await _context.Customer.FirstAsync(x => x.C_Code == customerCode);
    }
}
