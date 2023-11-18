using ECommerce.Application.DataTransferObjects.Color;

namespace ECommerce.Services.Services;

public class ColorService : EntityService<ColorReadDto, ColorCreateDto, ColorUpdateDto>, IColorService
{
    private const string Url = "api/Colors";
    private readonly IHttpService _http;

    public ColorService(IHttpService http) : base(http)
    {
        _http = http;
    }

    public async Task<ServiceResult<List<ColorReadDto>>> Load()
    {
        var result = await _http.GetAsync<List<ColorReadDto>>(Url, "GetAll");
        return Return(result);
    }

    public async Task<ServiceResult<List<ColorReadDto>>> GetAll(string search = "", int pageNumber = 0, int pageSize = 10)
    {
        var api = $"GetAllWithPagination?PaginationParameters.PageNumber={pageNumber}&PaginationParameters.Search={search}&PaginationParameters.PageSize={pageSize}";
        var result = await _http.GetAsync<List<ColorReadDto>>(Url, api);
        return Return(result);
    }

    public async Task<ServiceResult<List<ColorReadDto>>> Filtering(string filter)
    {
        var colors = await Load();
        if (colors.Code > 0) return colors;

        var result = colors.ReturnData.Where(x => x.Name.Contains(filter)).ToList();
        if (result.Count == 0)
            return new ServiceResult<List<ColorReadDto>> { Code = ServiceCode.Info, Message = "رنگ یافت نشد" };
        return new ServiceResult<List<ColorReadDto>>
        {
            Code = ServiceCode.Success,
            ReturnData = result
        };
    }

    public async Task<ServiceResult<ColorReadDto>> Add(ColorCreateDto color)
    {
        var response = await _http.PostAsync<ColorCreateDto, ColorReadDto>(Url, color);
        if (response == null)
            return new ServiceResult<ColorReadDto>
            {
                Code = ServiceCode.Error,
                Message = "سرور سایت در دسترس نیست. لطفا با پشتیبان سایت تماس بگیرید"
            };
        response.Messages = response.Code > 0
            ? new List<string> { response.GetBody() }
            : new List<string> { "با موفقیت ذخیره شد" };

        return Return(response);
    }

    public async Task<ServiceResult> Edit(ColorUpdateDto color)
    {
        var response = await _http.PutAsync<ColorUpdateDto>(Url, color);
        response.Messages = response.Code > 0
            ? new List<string> { response.GetBody() }
            : new List<string> { "با موفقیت ویرایش شد" };
        return Return(response);
    }

    public async Task<ServiceResult> Delete(int id)
    {
        //var result = await Delete(Url, id);
        //_colors = null;
        //return Return(result);
        var result = await _http.DeleteAsync(Url, id);
        if (result.Code == ResultCode.Success)
            return new ServiceResult
            {
                Code = ServiceCode.Success,
                Message = "با موفقیت حذف شد"
            };
        return new ServiceResult
            { Code = ServiceCode.Error, Message = "به علت وابستگی با عناصر دیگر امکان حذف وجود ندارد" };
    }

    public async Task<ServiceResult<ColorReadDto>> GetById(int id)
    {
        var result = await _http.GetAsync<ColorReadDto>(Url, $"GetById?id={id}");
        return Return(result);
    }
}