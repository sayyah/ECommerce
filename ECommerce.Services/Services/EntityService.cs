namespace ECommerce.Services.Services;

public class EntityService<TRead, TCreate, TUpdate>(IHttpService http) : IEntityService<TRead, TCreate, TUpdate>
{
    public async Task<ApiResult<List<TRead>>> ReadList(string url)
    {
        return await http.GetAsync<List<TRead>>(url);
    }

    public async Task<ApiResult<List<TRead>>> ReadList(string url, string api)
    {
        return await http.GetAsync<List<TRead>>(url, api);
    }

    public async Task<ApiResult<TRead>> Read(string url)
    {
        return await http.GetAsync<TRead>(url);
    }

    public async Task<ApiResult<TRead>> Read(string url, string api)
    {
        return await http.GetAsync<TRead>(url, api);
    }

    public async Task<ApiResult<object>> Create(string url, TCreate entity)
    {
        var response = await http.PostAsync(url, entity);
        if (response == null)
            return new ApiResult<object>
            {
                Code = ResultCode.ServerDontResponse,
                Messages = new List<string> { "سرور سایت در دسترس نیست. لطفا با پشتیبان سایت تماس بگیرید" }
            };
        response.Messages = response.Code > 0
            ? new List<string?> { response.GetBody() }
            : new List<string?> { "با موفقیت ذخیره شد" };
        return response;
    }

    public Task<ApiResult<TResponse>> Create<TResponse>(string url, TCreate entity)
    {
        throw new NotImplementedException();
    }


    public async Task<ApiResult> UpdateWithReturnId(string url, TUpdate entity)
    {
        var response = await http.PutAsync(url, entity);
        var messages = response.Code > 0
            ? new List<string?> { response.GetBody() }
            : new List<string?> { "با موفقیت ویرایش شد" };
        return new ApiResult
        {
            Code = ResultCode.Success,
            Messages = messages
        };
    }

    public async Task<ApiResult> Update(string url, TUpdate entity)
    {
        var response = await http.PutAsync(url, entity);
        response.Messages = response.Code > 0
            ? new List<string?> { response.GetBody() }
            : new List<string?> { "با موفقیت ویرایش شد" };
        return response;
    }
    public async Task<ApiResult> Update(string url, TUpdate entity, string apiName)
    {
        var response = await http.PutAsync(url, entity, apiName);
        if (response.Code > 0)
        {
            response.Messages = new List<string?> { response.GetBody() };
        }
        else
        {
            if (response.Messages?.Count() == 0) response.Messages = new List<string> { "با موفقیت ویرایش شد" };
        }

        return response;
    }

    public async Task<ApiResult> Delete(string url, int entityId)
    {
        var response = await http.DeleteAsync(url, entityId);
        response.Messages = response.Code > 0
            ? new List<string?> { response.GetBody() }
            : new List<string?> { "با موفقیت حذف شد" };
        return response;
    }

    public ServiceResult<TResult> Return<TResult>(ApiResult<TResult> result)
    {
        if (result is { Code: ResultCode.Success })
            return new ServiceResult<TResult>
            {
                PaginationDetails = result.PaginationDetails,
                Code = ServiceCode.Success,
                ReturnData = result.ReturnData,
                Message = result.Messages?.FirstOrDefault()
            };
        var typeOfTResult = Activator.CreateInstance(typeof(TResult));
        return new ServiceResult<TResult>
        {
            Code = ServiceCode.Error,
            Message = result.GetBody(),
            ReturnData = (TResult?)typeOfTResult
        };
    }

    public ServiceResult Return(ApiResult result)
    {
        if (result.Code == ResultCode.Success)
            return new ServiceResult
            {
                PaginationDetails = result.PaginationDetails,
                Code = ServiceCode.Success,
                Message = result.Messages?.FirstOrDefault()
            };
        return new ServiceResult
        {
            Code = ServiceCode.Error,
            Message = result.GetBody()
        };
    }

    public ServiceResult Return(ApiResult<object> result)
    {
        if (result.Code == ResultCode.Success)
            return new ServiceResult
            {
                PaginationDetails = result.PaginationDetails,
                Code = ServiceCode.Success,
                Message = result.Messages?.FirstOrDefault()
            };
        return new ServiceResult
        {
            Code = ServiceCode.Error,
            Message = result.GetBody()
        };
    }

    public async Task<ApiResult<object>> CreateWithoutToken(string url, TCreate entity)
    {
        var response = await http.PostAsyncWithoutToken(url, entity);
        response.Messages = response.Code > 0
            ? new List<string> { response.GetBody() }
            : new List<string> { "با موفقیت ذخیره شد" };
        return response;
    }
}