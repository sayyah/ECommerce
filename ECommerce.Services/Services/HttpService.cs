using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ECommerce.Services.Services;

public class HttpService(HttpClient http, ICookieService cookieService) : IHttpService
{
    private JsonSerializerOptions DefaultJsonSerializerOptions => new() { PropertyNameCaseInsensitive = true };

    public async Task<ApiResult<object>> PostAsyncWithoutToken<T>(string url, T data, string apiName = "Post")
    {
        var CreatorUserId = data.GetType().GetProperty("CreatorUserId");
        if (CreatorUserId != null)
        {
            var resultCurrentUser = cookieService.GetCurrentUser();
            if (CreatorUserId.GetValue(data) == null)
                CreatorUserId.SetValue(data, resultCurrentUser.Id);
        }

        var dataSerialize = JsonSerializer.Serialize(data);
        var content = new StringContent(dataSerialize, Encoding.UTF8, "application/json");
        var response = await http.PostAsync($"{url}/{apiName}", content);

        var responseDeserialized = await Deserialize<ApiResult<object>>(response, DefaultJsonSerializerOptions);
        return responseDeserialized;
    }

    public async Task<ApiResult<object>> PostAsync<T>(string url, T data, string apiName = "Post")
    {
        var loginResult = cookieService.GetToken();
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult);

        var CreatorUserId = data.GetType().GetProperty("CreatorUserId");
        if (CreatorUserId != null)
        {
            var resultCurrentUser = cookieService.GetCurrentUser();
            if (CreatorUserId.GetValue(data) == null)
                CreatorUserId.SetValue(data, resultCurrentUser.Id);
        }

        var dataSerialize = JsonSerializer.Serialize(data);
        var content = new StringContent(dataSerialize, Encoding.UTF8, "application/json");
        var response = await http.PostAsync($"{url}/{apiName}", content);

        var responseDeserialized = await Deserialize<ApiResult<object>>(response, DefaultJsonSerializerOptions);
        return responseDeserialized;
    }

    public async Task<ApiResult<TResponse>> PostAsync<T, TResponse>(string url, T data, string apiName = "Post")
    {
        var loginResult = cookieService.GetToken();
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult);

        var CreatorUserId = data.GetType().GetProperty("CreatorUserId");
        if (CreatorUserId != null)
        {
            var resultCurrentUser = cookieService.GetCurrentUser();
            if (CreatorUserId.GetValue(data) == null)
                CreatorUserId.SetValue(data, resultCurrentUser.Id);
        }

        var dataSerialize = JsonSerializer.Serialize(data);
        var content = new StringContent(dataSerialize, Encoding.UTF8, "application/json");
        var response = await http.PostAsync($"{url}/{apiName}", content);

        var responseData = new ResponseData<ApiResult<TResponse>>(default, false, response);
        if (response.IsSuccessStatusCode)
        {
            var responseDeserialized = await Deserialize<ApiResult<TResponse>>(response, DefaultJsonSerializerOptions);
            return responseDeserialized;
        }

        if (responseData.Response == null)
            return new ApiResult<TResponse> { Code = ResultCode.BadRequest };
        return responseData.Response;
    }

    public async Task<ApiResult> PutAsync<T>(string url, T data, string apiName = "Put")
    {
        var loginResult = cookieService.GetToken();
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult);

        var EditorUserId = data.GetType().GetProperty("EditorUserId");
        if (EditorUserId != null)
        {
            var resultCurrentUser = cookieService.GetCurrentUser();
            EditorUserId.SetValue(data, resultCurrentUser.Id);
        }

        var dataSerialize = JsonSerializer.Serialize(data);
        var content = new StringContent(dataSerialize, Encoding.UTF8, "application/json");
        var response = await http.PutAsync($"{url}/{apiName}", content);

        if (response.IsSuccessStatusCode)
        {
            var responseDeserialized = await Deserialize<ApiResult>(response, DefaultJsonSerializerOptions);
            return responseDeserialized;
        }

        return new ApiResult { Code = ResultCode.BadRequest };
    }

    public async Task<ApiResult> DeleteAsync(string url, int id, string apiName = "Delete")
    {
        var loginResult = cookieService.GetToken();
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult);

        var response = await http.DeleteAsync($"{url}/{apiName}?id={id}");

        var responseData = new ResponseData<ApiResult>(default, false, response);
        if (response.IsSuccessStatusCode)
        {
            var responseDeserialized = await Deserialize<ApiResult>(response, DefaultJsonSerializerOptions);
            return responseDeserialized;
        }

        return new ApiResult { Code = ResultCode.BadRequest };
    }

    public async Task<ApiResult<TResponse>> GetAsync<TResponse>(string url, string apiName = "Get")
    {
        var loginResult = cookieService.GetToken();
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult);

        var response = await http.GetAsync($"{url}/{apiName}");

        //HttpHeaders headers = response.Headers;
        //IEnumerable<string> values;
        //if (headers.TryGetValues("X-Pagination", out values))
        //{
        //    string session = values.First();
        //}

        var responseData = new ResponseData<ApiResult<TResponse>>(default, false, response);
        if (!response.IsSuccessStatusCode) return responseData.Response;

        var responseDeserialized = await Deserialize<ApiResult<TResponse>>(response, DefaultJsonSerializerOptions);
        return responseDeserialized;
    }

    public async Task<TResponse> PostAsyncWithApiKeyByRequestModel<TRequest, TResponse>(string apiName, string apiKey,
        TRequest data, string url)
    {
        http.DefaultRequestHeaders.Add(apiName, apiKey);
        var dataSerialize = JsonSerializer.Serialize(data);
        HttpContent content = new StringContent(dataSerialize, Encoding.UTF8, "application/json");
        var response = await http.PostAsync(url, content);
        var responseData = new ResponseData<ApiResult<TResponse>>(default, false, response);
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TResponse>(responseString, DefaultJsonSerializerOptions);
        return result;
    }

    public async Task<ApiResult<TResponse>> GetAsync<T, TResponse>(string url, T data, string apiName = "Get")
    {
        var loginResult = cookieService.GetToken();
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult);

        var dataSerialize = JsonSerializer.Serialize(data);
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
            Content = new StringContent(dataSerialize, Encoding.UTF8, "application/json")
        };
        var response = await http.SendAsync(request);

        //HttpHeaders headers = response.Headers;
        //IEnumerable<string> values;
        //if (headers.TryGetValues("X-Pagination", out values))
        //{
        //    string session = values.First();
        //}

        var responseData = new ResponseData<ApiResult<TResponse>>(default, false, response);
        if (!response.IsSuccessStatusCode) return responseData.Response;

        var responseDeserialized = await Deserialize<ApiResult<TResponse>>(response, DefaultJsonSerializerOptions);
        return responseDeserialized;
    }

    private async Task<T> Deserialize<T>(HttpResponseMessage httpResponse, JsonSerializerOptions options)
    {
        var responseString = await httpResponse.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseString, options);
    }
}