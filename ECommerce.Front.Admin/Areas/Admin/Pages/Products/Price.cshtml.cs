using ECommerce.Domain.Entities.HolooEntity;
using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Products;

public class PriceModel(IPriceService priceService, IUnitService unitService, ISizeService sizeService,
        IColorService colorService, ICurrencyService currencyService, IHolooMGroupService holooMGroupService,
        IHolooSGroupService holooSGroupService, IHolooArticleService holooArticleService)
    : PageModel
{
    public SelectList Units { get; set; }
    public SelectList Sizes { get; set; }
    public SelectList Colors { get; set; }
    public SelectList Currencies { get; set; }
    [BindProperty] public Price Price { get; set; } = new();
    public ServiceResult<List<Price>> Prices { get; set; }
    public List<HolooMGroup> HolooMGroups { get; set; } = new();
    public List<HolooSGroup> HolooSGroups { get; set; } = new();
    public List<Product> HolooArticle { get; set; } = new();

    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }

    public async Task OnGet(int id, string search = "", int pageIndex = 1, int quantityPerPage = 10,
        string message = null, string code = null)
    {
        Message = message;
        Code = code;

        await Initial(id, search, pageIndex, quantityPerPage, message, code);
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            ServiceResult result;
            if (Price.Id > 0)
                result = await priceService.Edit(Price);
            else
                result = await priceService.Add(Price);
            if (result.Code == 0)
                return RedirectToPage("/Products/Price",
                    new
                    {
                        area = "Admin",
                        id = Price.ProductId,
                        message = result.Message,
                        code = result.Code.ToString()
                    });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        await Initial(Price.ProductId);
        return Page();
    }

    public async Task<IActionResult> OnPostDelete(int id, int productId)
    {
        var result = await priceService.Delete(id);
        if (result.Code == 0)
            return RedirectToPage("/Products/Price",
                new { area = "Admin", id = productId, message = result.Message, code = result.Code.ToString() });
        Message = result.Message;
        Code = result.Code.ToString();
        await Initial(productId);
        return Page();
    }

    public async Task<IActionResult> OnPostEdit(int id, int productId)
    {
        await Initial(productId);
        Price = Prices.ReturnData.First(x => x.Id == id);
        return Page();
    }

    public async Task<JsonResult> OnGetReturnSGroup(string mCode)
    {
        var result = await holooSGroupService.Load(mCode);
        if (result.Code == ServiceCode.Success) return new JsonResult(result.ReturnData);

        return new JsonResult(new List<HolooSGroup>());
    }

    public async Task<JsonResult> OnGetReturnArticle(string code)
    {
        var result = await holooArticleService.Load(code);
        if (result.Code == ServiceCode.Success) return new JsonResult(result.ReturnData);

        return new JsonResult(new List<Product>());
    }

    private async Task Initial(int productId, string search = "", int pageNumber = 1, int pageSize = 10,
        string message = null, string code = null)
    {
        Price.ProductId = productId;
        var units = (await unitService.Load()).ReturnData;
        Units = new SelectList(units, nameof(Unit.Id), nameof(Unit.Name));

        var sizes = (await sizeService.Load()).ReturnData;
        Sizes = new SelectList(sizes, nameof(Size.Id), nameof(Size.Name));

        var colors = (await colorService.Load()).ReturnData;
        Colors = new SelectList(colors, nameof(Color.Id), nameof(Color.Name));

        var currencies = (await currencyService.Load()).ReturnData;
        Currencies = new SelectList(currencies, nameof(Currency.Id), nameof(Currency.Name));

        var result = await priceService.Load(productId.ToString(), pageNumber, pageSize);
        if (result.Code == ServiceCode.Success) Prices = result;

        HolooMGroups.Add(new HolooMGroup { M_groupname = "انتخاب گروه اصلی" });
        HolooMGroups.AddRange((await holooMGroupService.Load()).ReturnData);
        HolooSGroups.Add(new HolooSGroup { S_groupname = "ابتدا گروه اصلی را انتخاب کنید" });
        HolooArticle.Add(new Product { Name = "ابتدا گروه فرعی را انتخاب کنید" });
    }
}