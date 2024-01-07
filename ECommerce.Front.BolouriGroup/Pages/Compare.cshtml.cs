using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Pages;

public class CompareModel(ICompareService compareService) : PageModel
{
    public int CategoryId;

    public List<ProductCompareViewModel>? ProductsList { get; set; }
    public ProductCompareViewModel? CompareProduct { get; set; }
    public string Message { get; set; }

    public async Task<IActionResult> OnGetAsync(List<int> productListId)
    {
        Message = "";
        var result = await compareService.CompareList(productListId);
        if (result.ReturnData.First().ProductCategories.Count() > 0)
        {
            CategoryId = result.ReturnData.First().ProductCategories.First();
            var CategoriesResult = await compareService.GetProductsByCategories(CategoryId);
            ProductsList = CategoriesResult.ReturnData;
        }

        CompareProduct = result.ReturnData.First();
        if (ProductsList != null && ProductsList.Count() > 0)
        {
            var Index = ProductsList.FindIndex(a => a.Id == productListId.First());
            ProductsList.RemoveAt(Index);
        }

        return Page();
    }
}