using ECommerce.Domain.Entities.HolooEntity;
using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class HolooSarfaslRepository(HolooDbContext context) : HolooRepository<HolooSarfasl>(context),
    IHolooSarfaslRepository
{
    public async Task<string> Add(string username, CancellationToken cancellationToken)
    {
        var lastSarfaslCode = await context.Sarfasl.Where(c => c.Col_Code == "103")
            .OrderByDescending(x => x.Moien_Code).FirstOrDefaultAsync();
        var lastMoeinCode = lastSarfaslCode == null ? "0000" : lastSarfaslCode.Moien_Code;
        var newMoeinCode = (Convert.ToInt32(lastMoeinCode) + 1).ToString("D4");
        var sarfasl = new HolooSarfasl
        {
            Col_Code = "103",
            Moien_Code = newMoeinCode,
            Tafzili_Code = "",
            Sarfasl_Code = $"103{newMoeinCode}",
            Sarfasl_Name = username.Trim(),
            Mandeh = 0,
            Group = 1,
            Mahiat = 1,
            Can_Delete = false,
            AutoUse = false,
            Parent = 6,
            Type = 5,
            SParent = 0,
            ArzId = 1,
            Money_Price = 1,
            Selected = false
        };

         context.Sarfasl.Add(sarfasl);
        return newMoeinCode;
    }
}
