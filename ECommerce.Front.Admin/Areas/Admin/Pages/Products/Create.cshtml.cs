using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Products;

public class CreateModel(IProductService productService, ITagService tagService, ICategoryService categoryService,
        IHostEnvironment environment,
        IKeywordService keywordService, IBrandService brandService, IDiscountService discountService,
        IStoreService storeService,
        ISupplierService supplierService, IImageService imageService)
    : PageModel
{
    public SelectList Discounts { get; set; }
    public SelectList Stores { get; set; }
    public SelectList Suppliers { get; set; }
    public SelectList Brands { get; set; }
    public SelectList Tags { get; set; }
    public SelectList Keywords { get; set; }

    public List<CategoryParentViewModel> CategoryParentViewModel { get; set; }

    [BindProperty] public ProductViewModel Product { get; set; }
    [BindProperty] public List<IFormFile> Uploads { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }


    private async Task Initial()
    {
        //Product = new ProductViewModel();
        var stores = (await storeService.Load()).ReturnData;
        Stores = new SelectList(stores, nameof(Store.Id), nameof(Store.Name));

        var discounts = (await discountService.Load()).ReturnData;
        Discounts = new SelectList(discounts, nameof(Discount.Id), nameof(Discount.Name));

        var suppliers = (await supplierService.Load()).ReturnData;
        Suppliers = new SelectList(suppliers, nameof(Supplier.Id), nameof(Supplier.Name));

        var brands = (await brandService.Load()).ReturnData;
        Brands = new SelectList(brands, nameof(Brand.Id), nameof(Brand.Name));

        var tags = (await tagService.GetAll()).ReturnData;
        Tags = new SelectList(tags, nameof(Tag.Id), nameof(Tag.TagText));

        var keywords = (await keywordService.GetAll()).ReturnData;
        Keywords = new SelectList(keywords, nameof(Keyword.Id), nameof(Keyword.KeywordText));

        CategoryParentViewModel = (await categoryService.GetParents()).ReturnData;

        //HolooSGroups.AddRange((await _holooSGroupService.Load(HolooMGroups.First().M_groupcode)).ReturnData);
        //HolooArticle = (await _holooArticleService.Load($"{HolooSGroups.First().M_groupcode}{HolooSGroups.First().S_groupcode}")).ReturnData;
    }

    public async Task OnGet()
    {
        await Initial();
    }

    public async Task<IActionResult> OnPost()
    {
        if (Uploads == null)
        {
            Message = "لطفا عکس را انتخاب کنید";
            Code = ServiceCode.Error.ToString();
            return Page();
        }

        if (Product.CategoriesId.Count == 0)
        {
            Message = "لطفا حداقل یک دسته بندی انتخاب کنید";
            Code = "Error";
            await Initial();
            return Page();
        }

        foreach (var upload in Uploads)
            if (upload.FileName.Split('.').Last().ToLower() != "webp")
            {
                ModelState.AddModelError("IvalidFileExtention", "فرمت فایل پشتیبانی نمی‌شود.");
                await Initial();
                return Page();
            }

        if (ModelState.IsValid)
        {
            var result = await productService.Add(Product);
            if (result.Code == 0)
            {
                foreach (var upload in Uploads)
                {
                    var resultImage = await imageService.Add(upload, result.ReturnData.Id, "Images/Products",
                        environment.ContentRootPath);
                    if (resultImage.Code > 0)
                    {
                        Message = resultImage.Message;
                        Code = resultImage.Code.ToString();
                        ModelState.AddModelError("", resultImage.Message);
                        await Initial();
                        return Page();
                    }
                }

                return RedirectToPage("/Products/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            }

            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        await Initial();
        return Page();
    }


    public JsonResult OnGetChildrenCategory(int code, List<TreeViewModel> categoriesTreeViewModels)
    {
        var childrenCategory = categoriesTreeViewModels.Where(p => p.ParentId == code);

        return new JsonResult(childrenCategory);
    }
}