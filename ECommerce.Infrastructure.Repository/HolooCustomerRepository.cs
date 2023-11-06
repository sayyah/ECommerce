using ECommerce.Domain.Entities.HolooEntity;
<<<<<<< HEAD
using ECommerce.Infrastructure.DataContext;
=======
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

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

<<<<<<< HEAD
    public async Task<string> AddWithoutSave(HolooCustomer customer, CancellationToken cancellationToken)
    {
        await _context.Customer.AddAsync(customer, cancellationToken);     
=======
    public async Task<string> Add(HolooCustomer customer, CancellationToken cancellationToken)
    {
        await _context.Customer.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)
        return customer.C_Code;
    }

    public async Task<HolooCustomer> GetCustomerByCode(string customerCode)
    {
        return await _context.Customer.FirstAsync(x => x.C_Code == customerCode);
    }
<<<<<<< HEAD

    public async Task SaveAddedCustomer(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
=======
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)
}
