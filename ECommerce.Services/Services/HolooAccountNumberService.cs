using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Services.Services;

public class HolooAccountNumberService(IHttpService http) : EntityService<HolooAccountNumber, HolooAccountNumber, HolooAccountNumber>(http),
    IHolooAccountNumberService
{
    private const string Url = "api/PaymentMethods/HolooAccount";

    public List<HolooAccountNumber> HolooAccountNumbers { get; set; }

    public async Task Load()
    {
        HolooAccountNumbers = (await ReadList(Url)).ReturnData;
    }
}