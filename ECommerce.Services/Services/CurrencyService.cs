﻿namespace ECommerce.Services.Services;

public class CurrencyService(IHttpService http) : EntityService<Currency>(http), ICurrencyService
{
    private const string Url = "api/Currencies";
    private List<Currency> _currencies;


    public async Task<ServiceResult<List<Currency>>> Load(string search = "", int pageNumber = 0, int pageSize = 10)
    {
        var result = await ReadList(Url, $"Get?PageNumber={pageNumber}&PageSize={pageSize}&Search={search}");
        return Return(result);
    }

    public async Task<ServiceResult<List<Currency>>> Filtering(string filter)
    {
        if (_currencies == null)
        {
            var currencies = await Load();
            if (currencies.Code > 0) return currencies;
            _currencies = currencies.ReturnData;
        }

        var result = _currencies.Where(x => x.Name.Contains(filter)).ToList();
        if (result.Count == 0)
            return new ServiceResult<List<Currency>> { Code = ServiceCode.Info, Message = "ارزی یافت نشد" };
        return new ServiceResult<List<Currency>>
        {
            Code = ServiceCode.Success,
            ReturnData = result
        };
    }

    public async Task<ServiceResult> Add(Currency currency)
    {
        var result = await Create(Url, currency);
        _currencies = null;
        return Return(result);
    }

    public async Task<ServiceResult> Edit(Currency currency)
    {
        var result = await Update(Url, currency);
        _currencies = null;
        return Return(result);
    }

    public async Task<ServiceResult> Delete(int id)
    {
        //var result = await Delete(Url, id);
        //_currencies = null;
        //return Return(result);
        var result = await http.DeleteAsync(Url, id);
        if (result.Code == ResultCode.Success)
        {
            _currencies = null;
            return new ServiceResult
            {
                Code = ServiceCode.Success,
                Message = "با موفقیت حذف شد"
            };
        }

        _currencies = null;
        return new ServiceResult
            { Code = ServiceCode.Error, Message = "به علت وابستگی با عناصر دیگر امکان حذف وجود ندارد" };
    }

    public async Task<ServiceResult<Currency>> GetById(int id)
    {
        var result = await http.GetAsync<Currency>(Url, $"GetById?id={id}");
        return Return(result);
    }
}