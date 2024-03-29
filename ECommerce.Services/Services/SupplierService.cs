﻿namespace ECommerce.Services.Services;

public class SupplierService(IHttpService http) : EntityService<Supplier>(http), ISupplierService
{
    private const string Url = "api/Suppliers";
    private List<Supplier> _supplier;

    //
    public async Task<ServiceResult<List<Supplier>>> Load(string search = "", int pageNumber = 0, int pageSize = 10)
    {
        var result = await ReadList(Url, $"Get?PageNumber={pageNumber}&PageSize={pageSize}&Search={search}");
        return Return(result);
    }

    public async Task<ServiceResult<Dictionary<int, string>>> LoadDictionary()
    {
        var result = await ReadList(Url);
        if (result.Code == ResultCode.Success)
            return new ServiceResult<Dictionary<int, string>>
            {
                Code = ServiceCode.Success,
                ReturnData = result.ReturnData.ToDictionary(item => item.Id, item => item.Name),
                Message = result.Messages?.FirstOrDefault()
            };
        return new ServiceResult<Dictionary<int, string>>
        {
            Code = ServiceCode.Error,
            Message = result.GetBody()
        };
    }

    public async Task<ServiceResult<List<Supplier>>> Filtering(string filter)
    {
        if (_supplier == null)
        {
            var supplier = await Load();
            if (supplier.Code > 0) return supplier;
            _supplier = supplier.ReturnData;
        }

        var result = _supplier.Where(x => x.Name.Contains(filter)).ToList();
        if (result.Count == 0)
            return new ServiceResult<List<Supplier>> { Code = ServiceCode.Info, Message = "تامین کننده ای یافت نشد" };
        return new ServiceResult<List<Supplier>>
        {
            Code = ServiceCode.Success,
            ReturnData = result
        };
    }

    public async Task<ServiceResult> Add(Supplier supplier)
    {
        var result = await Create(Url, supplier);
        _supplier = null;
        return Return(result);
    }

    public async Task<ServiceResult> Edit(Supplier supplier)
    {
        var result = await Update(Url, supplier);
        _supplier = null;
        return Return(result);
    }

    public async Task<ServiceResult> Delete(int id)
    {
        var result = await http.DeleteAsync(Url, id);
        _supplier = null;
        if (result.Code == ResultCode.Success)
            return new ServiceResult
            {
                Code = ServiceCode.Success,
                Message = "با موفقیت حذف شد"
            };
        return new ServiceResult
            { Code = ServiceCode.Error, Message = "به علت وابستگی با عناصر دیگر امکان حذف وجود ندارد" };
    }

    public async Task<ServiceResult<Supplier>> GetById(int id)
    {
        var result = await http.GetAsync<Supplier>(Url, $"GetById?id={id}");
        return Return(result);
    }
}