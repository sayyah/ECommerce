﻿namespace ECommerce.Services.Services;

public class PaymentMethodService(IHttpService http) : EntityService<PaymentMethod>(http), IPaymentMethodService

{
    private const string Url = "api/PaymentMethods";
    private List<PaymentMethod> _paymentMethods;

    public async Task<ServiceResult<List<PaymentMethod>>> Load(string search = "", int pageNumber = 0,
        int pageSize = 10)
    {
        var result = await ReadList(Url, $"Get?PageNumber={pageNumber}&PageSize={pageSize}&Search={search}");
        return Return(result);
    }

    public async Task<ServiceResult<List<PaymentMethod>>> Filtering(string filter)
    {
        if (_paymentMethods == null)
        {
            var paymentMethods = await Load();
            if (paymentMethods.Code > 0) return paymentMethods;
            _paymentMethods = paymentMethods.ReturnData;
        }

        var result = _paymentMethods.Where(x => x.AccountNumber.Contains(filter)).ToList();
        if (result.Count == 0)
            return new ServiceResult<List<PaymentMethod>> { Code = ServiceCode.Info, Message = "شماره حسابی یافت نشد" };
        return new ServiceResult<List<PaymentMethod>>
        {
            Code = ServiceCode.Success,
            ReturnData = result
        };
    }

    public async Task<ServiceResult> Add(PaymentMethod paymentMethod)
    {
        var result = await Create(Url, paymentMethod);
        _paymentMethods = null;
        return Return(result);
    }

    public async Task<ServiceResult> Edit(PaymentMethod paymentMethod)
    {
        var result = await Update(Url, paymentMethod);
        _paymentMethods = null;
        return Return(result);
    }

    public async Task<ServiceResult> Delete(int id)
    {
        //var result = await Delete(Url, id);
        //_paymentMethods = null;
        //return Return(result);
        var result = await http.DeleteAsync(Url, id);
        if (result.Code == ResultCode.Success)
        {
            _paymentMethods = null;
            return new ServiceResult
            {
                Code = ServiceCode.Success,
                Message = "با موفقیت حذف شد"
            };
        }

        _paymentMethods = null;
        return new ServiceResult
            { Code = ServiceCode.Error, Message = "به علت وابستگی با عناصر دیگر امکان حذف وجود ندارد" };
    }

    public async Task<ServiceResult<PaymentMethod>> GetById(int id)
    {
        var result = await http.GetAsync<PaymentMethod>(Url, $"GetById?id={id}");
        return Return(result);
    }

    public async Task<ServiceResult> ConvertHolooToSunflower()
    {
        //await SweetMessage("ذخیره اتوماتیک", "لطفا جند لحظه صبر کنید", "warning");
        var result = await Create($"{Url}/ConvertHolooToSunflower", null);
        return Return(result);
    }
}