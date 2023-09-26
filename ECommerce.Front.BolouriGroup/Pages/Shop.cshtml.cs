using Ecommerce.Entities;
using Ecommerce.Entities.Helper;
using Ecommerce.Entities.ViewModel;
using ECommerce.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerce.Front.BolouriGroup.Pages
{

    public class ShopModel : PageModel
    {
        private readonly IBrandService _brandService;
        private readonly ICartService _cartService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ITagService _tagService;

        public ShopModel(ICategoryService categoryService, IProductService productService, IBrandService brandService,
            ICartService cartService, ITagService tagService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _brandService = brandService;
            _cartService = cartService;
            _tagService = tagService;
        }

        public ServiceResult<List<ProductIndexPageViewModel>> Products { get; set; }
        public ServiceResult<List<Tag>> Tags { get; set; }
        public Dictionary<int, string> Brands { get; set; }
        [BindProperty] public int Min { get; set; }
        [BindProperty] public int Max { get; set; }
        [BindProperty] public bool IsCheckExist { get; set; }
        [BindProperty] public int ProductSort { get; set; }
        [BindProperty] public string? Search { get; set; }
        public string CategoryBannerImagePath { get; set; }

        public async Task OnGet(string path, string? search = null, int pageNumber = 1, int pageSize = 20, int productSort = 1,
            string? message = null, string? code = null, string tagText = "", int minprice = 0, int maxprice = 0, bool isCheckExist = false)
        {
            Search = search;
            ProductSort = productSort;
            string[]? resultPath = path?.Split('=');
            IsCheckExist = isCheckExist;
            Min = minprice == 0 ? 1000 : minprice;
            Max = maxprice == 0 ? 200000000 : maxprice;
            if (resultPath != null && resultPath.Length > 0)
            {
                if (resultPath[0].Contains("tag"))
                {
                    tagText = resultPath[1];
                    path = null;
                }
            }

            string categoryId = "0";
            if (!string.IsNullOrEmpty(path))
            {
                var resultCategory = await _categoryService.GetByUrl(path);
                if (resultCategory.Code == ServiceCode.Success)
                {
                    categoryId = resultCategory.ReturnData.Id.ToString();
                    CategoryBannerImagePath = resultCategory.ReturnData.ImagePath;
                }
            }
            var searchByName = search;
            int articleCodeCustomer;
            if (int.TryParse(search, out articleCodeCustomer))
            {
                searchByName = $"articleCodeCustomer={search}";
            }
            else
                if (!string.IsNullOrEmpty(search) && !search.Contains('='))
                {
                    searchByName = $"Name={search}";
                }
            Products = await _productService.TopProducts(categoryId, searchByName, pageNumber, pageSize, productSort, maxprice, minprice, IsCheckExist, isWithoutBill: true, tagText: tagText);

            if (Products.Code == 0)
            {
                var brandResult = await _brandService.LoadDictionary();
                if (brandResult.Code == ServiceCode.Success) Brands = brandResult.ReturnData;

                Products.PaginationDetails.isCheckExist = isCheckExist;
                Products.PaginationDetails.MinPrice = minprice;
                Products.PaginationDetails.MaxPrice = maxprice;
                Products.PaginationDetails.ProductSort = productSort;
                Products.PaginationDetails.Search = search;
            }
        }

        public async Task<IActionResult> OnGetProducts(string path, string? search = null, int pageNumber = 1, int pageSize = 20, int productSort = 1,
            string? message = null, string? code = null, string tagText = "", int minprice = 0, int maxprice = 0, bool isCheckExist = false)
        {
            await OnGet(path, search, pageNumber, pageSize, productSort, message, code, tagText, minprice, maxprice, isCheckExist);
            return Partial("Components/_productCardList", Products.ReturnData);
        }

        public async Task<IActionResult> OnGetCounts(string path, string? search = null, int pageNumber = 1, int pageSize = 20, int productSort = 1,
            string? message = null, string? code = null, string tagText = "", int minprice = 0, int maxprice = 0, bool isCheckExist = false)
        {
            await OnGet(path, search, pageNumber, pageSize, productSort, message, code, tagText, minprice, maxprice, isCheckExist);
            var startNumber = (Products.PaginationDetails.CurrentPage - 1) * Products.PaginationDetails.PageSize + 1;
            var endNumber = Products.PaginationDetails.CurrentPage * Products.PaginationDetails.PageSize;
            var total = Products.PaginationDetails.TotalCount;
            if (endNumber > total) endNumber = total;
            return new JsonResult($"<p id=\"shop-result-count\">نمایش {startNumber} - {endNumber} از {total} نتیجه</p>");
        }

        public async Task<IActionResult> OnGetPagination(string path, string? search = null, int pageNumber = 1, int pageSize = 20, int productSort = 1,
            string? message = null, string? code = null, string tagText = "", int minprice = 0, int maxprice = 0, bool isCheckExist = false)
        {
            await OnGet(path, search, pageNumber, pageSize, productSort, message, code, tagText, minprice, maxprice, isCheckExist);
            return Partial("_Pagination", Products.PaginationDetails);
        }

        public async Task<IActionResult> OnGetSearch([FromQuery] Request request)
        {
            string SearchText;
            int articleCodeCustomer;
            if (int.TryParse(request.SearchText, out articleCodeCustomer))
            {
                SearchText = $"articleCodeCustomer={request.SearchText}";
            }
            else
            {
                SearchText = $"Name={request.SearchText}";
            }
            var resultSearchProducts = await _productService.TopProducts("", SearchText, request.Page, request.QuantityPerPage, 1, null, null, false, true, "");
            Search = request.SearchText;
            return new JsonResult(resultSearchProducts.ReturnData);
        }


        public async Task<IActionResult> OnGetSearchCategory([FromQuery] Request request)
        {
            var resultSearchCategories = await _categoryService.Search(request.SearchText);
            foreach (var resultSearchCategory in resultSearchCategories.ReturnData)
            {
                if (string.IsNullOrEmpty(resultSearchCategory.ImagePath))
                {
                    resultSearchCategory.ImagePath = "/img/BlueLogo.webp";
                }
            }
            return new JsonResult(resultSearchCategories.ReturnData);
        }
    }

    public class Request
    {
        public int Page { get; set; }
        public int QuantityPerPage { get; set; }
        public string? SearchText { get; set; }
    }
}