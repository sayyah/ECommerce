using ECommerce.Application.ViewModels;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Services.IServices;

public interface ICompareService
{
    ServiceResult Remove(HttpContext context, int productId);
    Task<ServiceResult<List<ProductCompareViewModel>>> CompareList(List<int> productId);
    Task<ServiceResult<List<ProductCompareViewModel>>> GetProductsByCategories(int categoryId);
}