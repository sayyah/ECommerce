using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Infrastructure.Repository;

public class HolooAccountNumberRepository(HolooDbContext context) : HolooRepository<HolooAccountNumber>(context),
    IHolooAccountNumberRepository
{
    public async Task<HolooAccountNumber?> GetByAccountNumberAndBankCode(string code,
        CancellationToken cancellationToken)
    {
        var temp = code.Split("-");
        var bankCode = temp[0];
        var accountNumber = temp[1];

        return await context.ACOUND_N.Where(x =>
                x.Account_N.Equals(accountNumber) && x.Bank_Code.Equals(bankCode) && x.C_Code.Equals("00000"))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
