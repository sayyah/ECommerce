using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Infrastructure.Repository;

public class HolooCustomerRepository(HolooDbContext context) : HolooRepository<HolooCustomer>(context),
    IHolooCustomerRepository
{
    public async Task<(string customerCode, string customerCodeC)> GetNewCustomerCode()
    {
        var customer = await context.Customer.OrderByDescending(x => x.C_Code).FirstOrDefaultAsync();
        var customerCode = customer == null ? "00000" : customer.C_Code;
        var customerCodeC = customer == null ? "00000" : customer.C_Code_C;
        return ((Convert.ToInt32(customerCode) + 1).ToString("D5"), (Convert.ToInt32(customerCodeC) + 1).ToString());
    }

    public async Task<string> AddWithoutSave(HolooCustomer customer, CancellationToken cancellationToken)
    {
        await context.Customer.AddAsync(customer, cancellationToken);     
        return customer.C_Code;
    }

    public async Task<HolooCustomer> GetCustomerByCode(string customerCode)
    {
        return await context.Customer.FirstAsync(x => x.C_Code == customerCode);
    }
}
