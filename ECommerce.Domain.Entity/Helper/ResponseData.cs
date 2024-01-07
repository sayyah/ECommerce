namespace ECommerce.Domain.Entities.Helper;

public class ResponseData<T>(T response, bool success, HttpResponseMessage httpResponseMessage)
{
    public bool Success { get; set; } = success;
    public T Response { get; set; } = response;
    public HttpResponseMessage HttpResponseMessage { get; set; } = httpResponseMessage;

    public async Task<string> GetBody()
    {
        return await HttpResponseMessage.Content.ReadAsStringAsync();
    }
}
