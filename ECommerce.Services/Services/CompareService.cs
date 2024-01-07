using ECommerce.Application.ViewModels;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Services.Services;

public class CompareService(ICookieService cookieService, IProductService productService)
    : ICompareService
{
    private readonly string _key = "Compare";

    public ServiceResult Remove(HttpContext context, int productId)
    {
        cookieService.Remove(context, new CookieData($"{_key}-{productId}", productId));
        return new ServiceResult { Code = ServiceCode.Success, Message = "کالا از لیست مقایسه حذف شد" };
    }

    public async Task<ServiceResult<List<ProductCompareViewModel>>> CompareList(List<int> productIdList)
    {
        var result = await productService.ProductsWithIdsForCompare(productIdList);
        return result;
    }

    public async Task<ServiceResult<List<ProductCompareViewModel>>> GetProductsByCategories(int categoryId)
    {
        var result = await productService.ProductsWithCategoriesForCompare(categoryId);
        return result;
    }
}