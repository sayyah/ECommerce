﻿namespace ECommerce.Services.Services;

public class StarService(IHttpService http, ICookieService cookieService) : EntityService<ProductUserRank>(http),
    IStarService
{
    private const string Url = "api/Stars";

    public int FillStars { get; set; }
    public int EmptyStars { get; set; }
    public int StarCount { get; set; }

    public async Task<ApiResult<List<ProductUserRank>>> Load()
    {
        var currentUser = cookieService.GetCurrentUser();
        var temp = await http.GetAsync<List<ProductUserRank>>(Url, $"GetByUserId?id={currentUser.Id}");
        return temp;
    }

    public async Task<ServiceResult> SaveStars(int productId, int starNumber)
    {
        var currentUser = cookieService.GetCurrentUser();
        if (currentUser.Id == 0)
            return new ServiceResult
            {
                Message = "برای امتیاز دادن ابتدا لاگین کنید",
                Code = ServiceCode.Error
            };
        var productUserRank = new ProductUserRank
        {
            ProductId = productId,
            UserId = currentUser.Id,
            Stars = starNumber
        };
        var result = await Create(Url, productUserRank);
        return new ServiceResult
        {
            Message = result.Code == 0
                ? "امتیاز شما با موفقیت ذخیره شد"
                : result.Messages != null && result.Messages.Any()
                    ? result.Messages.First()
                    : "متاسفانه امتیاز شما ذخیره نشده. به پشتیبانی سایت اطلاع دهید",
            Code = result.Code == 0 ? ServiceCode.Success : ServiceCode.Error
        };
    }

    public async Task<double> SumStarsByProductId(int productId)
    {
        var result = await http.GetAsync<double>(Url, $"GetBySumProductId?id={productId}");
        return result.Code == 0 ? result.ReturnData : 0;
    }
}