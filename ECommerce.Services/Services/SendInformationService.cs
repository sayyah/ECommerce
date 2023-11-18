namespace ECommerce.Services.Services;

public class SendInformationService
    (IHttpService http, ICookieService cookieService) : EntityService<SendInformation, SendInformation, SendInformation>(http), ISendInformationService
{
    private const string Url = "api/SendInformation";

    public async Task<ServiceResult<List<SendInformation>>> Load()
    {
        var currentUser = cookieService.GetCurrentUser();
        var result = await ReadList(Url, $"GetByUserId?id={currentUser.Id}");
        return Return(result);
    }

    public async Task<ServiceResult<SendInformation>> Find(int id)
    {
        var result = await Read(Url, $"GetById?id={id}");
        return Return(result);
    }

    public async Task<ServiceResult<SendInformation>> Add(SendInformation sendInformation)
    {
        var currentUser = cookieService.GetCurrentUser();
        sendInformation.UserId = currentUser.Id;
        var result = await Create<SendInformation>(Url, sendInformation);
        return Return(result);
    }

    public async Task<ServiceResult> Edit(SendInformation sendInformation)
    {
        var result = await Update(Url, sendInformation);
        return Return(result);
    }

    public async Task<ServiceResult> Delete(int id)
    {
        var result = await Delete(Url, id);
        return Return(result);
    }
}