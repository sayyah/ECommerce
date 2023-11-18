using ECommerce.Application.ViewModels;

namespace ECommerce.Services.Services;

public class WishListService(IHttpService http, ICookieService cookieService) : EntityService<WishList, WishList, WishList>(http),
    IWishListService
{
    private const string Url = "api/WishLists";

    public async Task<ServiceResult<List<WishListViewModel>>> Load()
    {
        var currentUser = cookieService.GetCurrentUser();

        if (currentUser.Id != 0)
        {
            var response = await http.GetAsync<List<WishListViewModel>>(Url, $"GetById?id={currentUser.Id}");

            return new ServiceResult<List<WishListViewModel>>
            {
                Code = ServiceCode.Success,
                Message = "لیست علاقمندی ها",
                ReturnData = response.ReturnData
            };
        }

        return new ServiceResult<List<WishListViewModel>>
        {
            Code = ServiceCode.Info,
            Message = "لطفا ابتدا وارد شوید"
        };
    }

    public async Task<ServiceResult> Invert(int priceId)
    {
        var currentUser = cookieService.GetCurrentUser();
        if (currentUser.Id == 0)
            return new ServiceResult
            {
                Code = ServiceCode.Info,
                Message = "لطفا ابتدا وارد شوید"
            };
        var wishList = new WishList
        {
            PriceId = priceId,
            UserId = currentUser.Id
        };
        var response = await http.PutAsync(Url, wishList, "Invert");

        return new ServiceResult
        {
            Code = response != null ? ServiceCode.Success : ServiceCode.Error,
            Message = response.Messages.FirstOrDefault()
        };
    }

    public async Task<ServiceResult> Add(int priceId)
    {
        var currentUser = cookieService.GetCurrentUser();
        if (currentUser.Id == 0)
            //await _sweet.FireAsync(, "Info");
            return new ServiceResult
            {
                Code = ServiceCode.Info,
                Message = "لطفا ابتدا وارد شوید"
            };
        var wishList = new WishList
        {
            PriceId = priceId,
            UserId = currentUser.Id
        };
        var result = await Create(Url, wishList);
        if (result.Code == 0)
            return new ServiceResult
            {
                Code = ServiceCode.Success,
                Message = "علاقمندی شما با موفقیت ذخیره شد"
            };
        return new ServiceResult
        {
            Code = ServiceCode.Error,
            Message = "افزودن علاقمندی با مشکل مواجه شد"
        };
    }

    public async Task<ServiceResult> Delete(int wishListId)
    {
        var deleteResult = await http.DeleteAsync(Url, wishListId);
        if (deleteResult.Code == 0)
            return new ServiceResult
            {
                Code = ServiceCode.Success,
                Message = "از لیست علاقه مندی شما حذف شد"
            };
        //await _sweet.FireAsync("حذف", "با موفقیت حذف شد", "success");

        return new ServiceResult
        {
            Code = ServiceCode.Error,
            Message = "در حذف مشکل پیش آمد"
        };
    }
}