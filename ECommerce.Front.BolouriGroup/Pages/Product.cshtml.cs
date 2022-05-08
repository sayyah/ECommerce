using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.Helper;
using Entities.ViewModel;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Services.IServices;

namespace Bolouri.Pages
{
    public class ProductdetailsModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IStarService _starService;
        private readonly ICartService _cartService;
        private readonly ITagService _tagService;
        private readonly IProductAttributeGroupService _attributeGroupService;

        public ProductViewModel Product { get; set; }
        public List<Entities.Tag> tags { get; set; }
        public List<ProductIndexPageViewModel> RelatedProducts { get; set; }
        public double Stars { get; set; }
        public List<ProductAttributeGroup> AttributeGroups { get; set; }

        public ProductdetailsModel(IProductService productService, IStarService starService, ICartService cartService,ITagService tagService, IProductAttributeGroupService attributeGroupService)
        {
            _productService = productService;
            _starService = starService;
            _cartService = cartService;
            _tagService = tagService;
            _attributeGroupService = attributeGroupService;
        }
        public async Task OnGet(string productUrl)
        {
            var resultProduct = await _productService.GetProduct(productUrl);
            if (resultProduct.Code == 0) Product = resultProduct.ReturnData;

            var result = await _attributeGroupService.GetByProductId(Product.Id);
            if (result.Code == ServiceCode.Success)
            {
                AttributeGroups = result.ReturnData.Where(x=>
                    x.Attribute.Any(a=>
                        a.AttributeValue.Any(i=>
                            i.Value!=null))).ToList();

            }

            RelatedProducts = (await _productService.TopRelativesProduct(Product.Id)).ReturnData;

            Stars = await _starService.SumStarsByProductId(Product.Id);
        }
        public async Task<JsonResult> OnGetAddCart(int id)
        {
            var result = await _cartService.Add(HttpContext, id);
            return new JsonResult(result);
        }

        public async Task<JsonResult> OnGetLoadCart(int id)
        {
            var result = await _cartService.Load(HttpContext);
            return new JsonResult(result);
        }

        public async Task<JsonResult> OnGetDeleteCart(int id)
        {
            var result = await _cartService.Delete(HttpContext, id);
            return new JsonResult(result);
        }
    }
}
