﻿namespace ECommerce.Services.Services;

public class CityService(IHttpService http) : EntityService<City>(http), ICityService
{
    private const string Url = "api/Cities";
    private List<City> _cities;

    public async Task<ServiceResult<List<City>>> LoadAllCity()
    {
        var result = await ReadList(Url, "GetAll");
        return Return(result);
    }

    public async Task<ServiceResult<List<City>>> Load(int id)
    {
        var result = await ReadList(Url, $"GetById?id={id}");
        return Return(result);
    }

    public async Task<ServiceResult<List<City>>> GetWithPagination(string search = "", int pageNumber = 0,
        int pageSize = 10)
    {
        var result = await ReadList(Url,
            $"GetAllWithPagination?PageNumber={pageNumber}&PageSize={pageSize}&Search={search}");
        return Return(result);
    }


    public async Task<ServiceResult<List<City>>> Filtering(string filter, int id)
    {
        if (_cities == null)
        {
            var cities = await Load(id);
            if (cities.Code > 0) return cities;
            _cities = cities.ReturnData;
        }

        var result = _cities.Where(x => x.Name.Contains(filter)).ToList();
        if (result.Count == 0)
            return new ServiceResult<List<City>> { Code = ServiceCode.Info, Message = "شهر یافت نشد" };
        return new ServiceResult<List<City>>
        {
            Code = ServiceCode.Success,
            ReturnData = result
        };
    }

    public async Task<ServiceResult> Add(City city)
    {
        var result = await Create(Url, city);
        _cities = null;
        return Return(result);
    }

    public async Task<ServiceResult> Edit(City city)
    {
        var result = await Update(Url, city);
        _cities = null;
        return Return(result);
    }

    public async Task<ServiceResult> Delete(int id)
    {
        //var result = await Delete(Url, id);
        //_cities = null;
        //return Return(result);
        var result = await http.DeleteAsync(Url, id);
        if (result.Code == ResultCode.Success)
            return new ServiceResult
            {
                Code = ServiceCode.Success,
                Message = "با موفقیت حذف شد"
            };
        return new ServiceResult
            { Code = ServiceCode.Error, Message = "به علت وابستگی با عناصر دیگر امکان حذف وجود ندارد" };
    }

    public async Task<ServiceResult<City>> GetByStateId(int id)
    {
        var result = await http.GetAsync<City>(Url, $"GetByStateId?id={id}");
        return Return(result);
    }

    public async Task<ServiceResult<City>> GetById(int id)
    {
        var result = await http.GetAsync<City>(Url, $"GetById?id={id}");
        return Return(result);
    }
}