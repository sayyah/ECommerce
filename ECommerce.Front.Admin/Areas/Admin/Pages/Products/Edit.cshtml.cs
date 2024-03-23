using ECommerce.API.DataTransferObject.Keywords;
using ECommerce.API.DataTransferObject.Tags;
using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Products;

public class EditModel(IProductService productService, ITagService tagService, ICategoryService categoryService,
        IHostEnvironment environment,
        IKeywordService keywordService, IBrandService brandService, IDiscountService discountService,
        IStoreService storeService,
        ISupplierService supplierService, IImageService imageService)
    : PageModel
{
    public List<Discount> Discounts { get; set; }
    public List<Store> Stores { get; set; }
    public List<Supplier> Suppliers { get; set; }
    public List<Brand> Brands { get; set; }
    public List<ReadTagDto>? Tags { get; set; }
    public List<ReadKeywordDto>? Keywords { get; set; }

    public List<CategoryParentViewModel>? CategoryParentViewModel { get; set; }

    [BindProperty] public ProductViewModel Product { get; set; }
    [BindProperty] public List<IFormFile> Uploads { get; set; }
    [TempData] public string? Message { get; set; }
    [TempData] public string? Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await Initial(id);
        if (result.Code == 0) return Page();
        return RedirectToPage("/Products/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost()
    {
        if (Product.CategoriesId.Count == 0)
        {
            Message = "لطفا حداقل یک دسته بندی انتخاب کنید";
            Code = "Error";
            await Initial(Product.Id);
            return Page();
        }

        if (Uploads == null)
        {
            Message = "لطفا عکس را انتخاب کنید";
            Code = ServiceCode.Error.ToString();
            await Initial(Product.Id);
            return Page();
        }

        foreach (var upload in Uploads)
            if (upload.FileName.Split('.').Last().ToLower() != "webp")
            {
                ModelState.AddModelError("IvalidFileExtention", "فرمت فایل پشتیبانی نمی‌شود.");
                await Initial(Product.Id);
                return Page();
            }

        if (ModelState.IsValid)
        {
            var result = await productService.Edit(Product);
            if (result.Code == 0)
            {
                foreach (var upload in Uploads)
                {
                    var resultImage = await imageService.Add(upload, Product.Id, "Images/Products",
                        environment.ContentRootPath);
                    if (resultImage.Code > 0)
                    {
                        Message = resultImage.Message;
                        Code = resultImage.Code.ToString();
                        ModelState.AddModelError("", resultImage.Message);
                        await Initial(Product.Id);
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

        await Initial(Product.Id);
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteImage(string imageName, int id, int productId)
    {
        {
            var result = await imageService.Delete($"Images/Products/{imageName}", id, environment.ContentRootPath);

            if (result.Code == 0)
                return RedirectToPage("/Products/Edit",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }
        await Initial(productId);
        return Page();
    }

    private async Task<ServiceResult<ProductViewModel>> Initial(int id)
    {
        var result = await productService.GetById(id);
        if (result.Code > 0)
            return new ServiceResult<ProductViewModel>
            {
                Code = result.Code,
                Message = result.Message
            };
        Product = result.ReturnData;

        //Product = new ProductViewModel();
        Stores = (await storeService.Load()).ReturnData;

        Discounts = (await discountService.Load()).ReturnData;

        Suppliers = (await supplierService.Load()).ReturnData;

        Brands = (await brandService.Load()).ReturnData;

        Tags = (await tagService.GetAll()).ReturnData;

        Keywords = (await keywordService.GetAll()).ReturnData;

        CategoryParentViewModel = (await categoryService.GetParents(id)).ReturnData;

        return result;
    }
}