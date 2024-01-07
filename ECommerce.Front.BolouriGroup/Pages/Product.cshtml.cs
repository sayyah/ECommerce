using ECommerce.Front.BolouriGroup.Models;
using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Http.Extensions;

namespace ECommerce.Front.BolouriGroup.Pages;

public class ProductdetailsModel(IProductService productService, IStarService starService, ICartService cartService,
        ITagService tagService, IProductAttributeGroupService attributeGroupService, IUserService userService
        , IProductCommentService productCommandService, IHttpContextAccessor httpContextAccessor,
        IWishListService wishListService)
    : PageModel
{
    private readonly ICartService _cartService = cartService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ITagService _tagService = tagService;
    private readonly IWishListService _wishListService = wishListService;

    public string siteUrl { get; set; }
    public ProductViewModel Product { get; set; }
    public int? WishListPriceId { get; set; }
    public List<Tag> tags { get; set; }
    public List<ProductIndexPageViewModel> RelatedProducts { get; set; }
    public double Stars { get; set; }
    public List<ProductAttributeGroup> AttributeGroups { get; set; }
    public ProductComment? ProductComment { get; set; }
    [BindProperty] public string Message { get; set; }
    [BindProperty] public string Code { get; set; }
    public ServiceResult<List<ProductComment>> ProductComments { get; set; }

    private async Task Initial(string productUrl, int pageNumber = 1, int pageSize = 10)
    {
        var user = await userService.GetUser();
        var resultProduct = await productService.GetProduct(productUrl, user.ReturnData.Id);
        if (resultProduct.Code > 0) return;
        Product = resultProduct.ReturnData;
        WishListPriceId = resultProduct.ReturnData.WishListPriceId;
        var result = await attributeGroupService.GetByProductId(Product.Id);
        if (result.Code == ServiceCode.Success)
            AttributeGroups = result.ReturnData.Where(x =>
                x.Attribute.Any(a =>
                    a.AttributeValue.Any(i =>
                        i.Value != null))).ToList();

        RelatedProducts = (await productService.TopRelatives(Product.Id)).ReturnData;

        Stars = await starService.SumStarsByProductId(Product.Id);

        ProductComments =
            await productCommandService.GetAllAccesptedComments(Convert.ToString(Product.Id), pageNumber, pageSize);

        var url = HttpContext.Request.GetDisplayUrl().Split("/");
        siteUrl = string.Format("{0}//{1}", url[0], url[2]);
    }

    public async Task<IActionResult> OnGet(string productUrl, int pageNumber = 1, int pageSize = 10)
    {
        await Initial(productUrl, pageNumber, pageSize);
        if(Product == null)
        {
            return RedirectToPage("Error");
        }
        return Page();
    }

    public async Task<IActionResult> OnGetComment(string productUrl, string name, string email, string text)
    {
        VerifyResultData resultData = new();

        if (string.IsNullOrEmpty(name))
        {
            resultData.Description = "لطفا نام خود را برای ثبت نظر وارد کنید";
            resultData.Succeed = false;
            return new JsonResult(resultData);
        }

        if (string.IsNullOrEmpty(email))
        {
            resultData.Description = "لطفا ایمیل خود را برای ثبت نظر وارد کنید";
            resultData.Succeed = false;
            return new JsonResult(resultData);
        }

        if (string.IsNullOrEmpty(text))
        {
            resultData.Description = "لطفا نظر خود را برای ثبت نظر وارد کنید";
            resultData.Succeed = false;
            return new JsonResult(resultData);
        }

        ProductComment productComment = new()
        {
            Email = email,
            Name = name,
            Text = text,
            User = null,
            UserId = null
        };

        var user = await userService.GetUser();
        if (user.Code == 0) productComment.UserId = user.ReturnData.Id;

        var resultProduct = await productService.GetProduct(productUrl, user.ReturnData.Id);
        if (resultProduct.Code > 0)
        {
            resultData.Succeed = false;
            resultData.Description = "ثبت نظر با مشکل مواجه شد. لطفا مجددا تست کنید";
            return new JsonResult(resultData);
        }

        Product = resultProduct.ReturnData;
        productComment.ProductId = Product.Id;

        var result = await productCommandService.Add(productComment);
        if (result.Code == 0)
        {
            resultData.Description = "نظر شما ثبت شد، پس از تایید توسط ادمین سایت، نمایش داده می شود";
            resultData.Succeed = true;
        }
        else
        {
            resultData.Description = "ثبت نظر با مشکل مواجه شد. لطفا مجددا تست کنید";
            resultData.Succeed = false;
        }

        return new JsonResult(resultData);
    }

    public IActionResult OnGetAddCompareList(int id)
    {
        var productListId = new List<int>();
        productListId.Add(id);
        return RedirectToPage("/Compare", new { productListId });
    }
}