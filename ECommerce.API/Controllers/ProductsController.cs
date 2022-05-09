﻿using Microsoft.AspNetCore.Mvc;
using API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Utilities;
using Entities;
using Entities.Helper;
using Entities.HolooEntity;
using Entities.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IHolooArticleRepository _articleRepository;
        private readonly IHolooMGroupRepository _mGroupRepository;
        private readonly IHolooSGroupRepository _sGroupRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPriceRepository _priceRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository productRepository, IHolooArticleRepository articleRepository, IHolooMGroupRepository mGroupRepository, IHolooSGroupRepository sGroupRepository, ICategoryRepository categoryRepository, IPriceRepository priceRepository, IUnitRepository unitRepository, ILogger<ProductsController> logger)
        {
            this._productRepository = productRepository;
            _articleRepository = articleRepository;
            _mGroupRepository = mGroupRepository;
            _sGroupRepository = sGroupRepository;
            _categoryRepository = categoryRepository;
            _priceRepository = priceRepository;
            _unitRepository = unitRepository;
            _logger = logger;
        }

        private async Task<List<Product>> AddPriceAndExistFromHolooList(List<Product> products)
        {
            foreach (var product in products.Where(x => x.Prices.Any(p => p.ArticleCode != null)))
            {
                foreach (var productPrices in product.Prices)
                {

                    if (productPrices.SellNumber != null && productPrices.SellNumber != Price.HolooSellNumber.خالی)
                    {
                        var article = await _articleRepository.GetHolooPrice(productPrices.ArticleCode,
                            productPrices.SellNumber!.Value);
                        productPrices.Amount = article.price / 10;
                        productPrices.Exist = (double)article.exist;
                    }
                }
            }
            return products;
        }
        private async Task<Product> AddPriceAndExistFromHoloo(Product product)
        {
            foreach (var productPrices in product.Prices)
            {

                if (productPrices.SellNumber != null && productPrices.SellNumber != Price.HolooSellNumber.خالی)
                {
                    var article = await _articleRepository.GetHolooPrice(productPrices.ArticleCode,
                        productPrices.SellNumber!.Value);
                    productPrices.Amount = article.price / 10;
                    productPrices.Exist = (double)article.exist;
                }
            }

            return product;
        }
        [HttpPost]
        public async Task<IActionResult> Search(PageViewModel pageViewModel, CancellationToken cancellationToken)
        {
            try
            {
                var pagination = new PaginationViewModel { SearchText = pageViewModel.SearchText };
                var query = _productRepository.GetWithInclude(cancellationToken);

                var cox = query.Count();
                if (string.IsNullOrEmpty(pageViewModel.SearchText))
                {
                    pagination.Products = await query.Paginate(pageViewModel);
                    pagination.ProductsCount = query.Count();
                    pagination.Page = pageViewModel.Page;
                    pagination.QuantityPerPage = pageViewModel.QuantityPerPage;
                    var temp = (double)pagination.ProductsCount / pageViewModel.QuantityPerPage;
                    pagination.PagesQuantity = (int)Math.Ceiling(temp);

                    if (pagination.Products.Any(x => x.Prices.Any(p => p.ArticleCode != null)))
                    {
                        pagination.Products = await AddPriceAndExistFromHolooList(pagination.Products);
                    }
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.Success,
                        ReturnData = pagination
                    });
                }

                //if (pageViewModel.SearchText.StartsWith("@"))
                //{
                pagination.QuantityPerPage = pageViewModel.QuantityPerPage;
                pagination.Products = await query.Where(x => x.Name.Contains(pageViewModel.SearchText.Substring(1))).Paginate(pageViewModel);
                pagination.ProductsCount = query.Count(x => x.Name.Contains(pageViewModel.SearchText.Substring(1)));
                pagination.PagesQuantity = (int)Math.Ceiling((double)(pagination.ProductsCount / pageViewModel.QuantityPerPage));
                pagination.Page = pageViewModel.Page;

                if (pagination.Products.Any(x => x.Prices.Any(p => p.ArticleCode != null)))
                {
                    pagination.Products = await AddPriceAndExistFromHolooList(pagination.Products);
                }
                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = pagination
                });
                //}
                ////var categoriesId = _categoryRepository.GetIdsByUrl(paginationViewModel.SearchText);
                //var productList = query.Where(x => x.ProductCategories.Any(y => y.Path == pageViewModel.SearchText));
                //if(!productList.Any())
                //    return Ok(new ApiResult
                //    {
                //        Code = ResultCode.NotFound
                //    });
                //pagination.QuantityPerPage = pageViewModel.QuantityPerPage;
                //pagination.Page = pageViewModel.Page;
                //pagination.Products =await productList.Paginate(pageViewModel);
                //pagination.ProductsCount = query.Count(x => Enumerable.Any<Category>(x.ProductCategories, y => y.Path == pageViewModel.SearchText));
                //pagination.PagesQuantity = (int)Math.Ceiling((double)(pagination.ProductsCount / pageViewModel.QuantityPerPage));
                //if (pagination.Products.Count == 0) return NotFound("دسته بندی موجود نیست");
                //return Ok(new ApiResult
                //{
                //    Code = ResultCode.Success,
                //    ReturnData = pagination
                //});
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productListFilteredViewModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductListFilteredViewModel productListFilteredViewModel, CancellationToken cancellationToken)
        {
            try
            {
                //var products = await _productRepository.TopNew(Convert.ToInt32(productListFilteredViewModel.PaginationParameters.Search), cancellationToken);

                var products = new List<Product>();
                var productQuery = _productRepository.GetProducts(productListFilteredViewModel.BrandsId,
                    productListFilteredViewModel.StarsCount, productListFilteredViewModel.TagsId);
                var search = productListFilteredViewModel.PaginationParameters.Search?.Split('=');
                if (search is {Length: > 1})
                {
                    switch (search[0])
                    {
                        case "CategoryId":
                            var categoriesId = new List<int>();
                            categoriesId = await _categoryRepository.ChildrenCategory(Convert.ToInt32(search[1]), cancellationToken);
                            foreach (var categoryId in categoriesId)
                            {
                                products.AddRange(await productQuery
                                    .Where(x => x.ProductCategories.Any(y => y.Id == categoryId))
                                    .ToListAsync(cancellationToken));
                            }
                            break;
                        case "BrandId":
                            products.AddRange(await productQuery
                                .Where(x => x.BrandId== Convert.ToInt32(search[1]))
                                .ToListAsync(cancellationToken));
                            break;
                    }
                  
                }
                else
                {
                    products =await productQuery.ToListAsync(cancellationToken);
                }

                if (products.Any(x => x.Prices.Any(p => p.ArticleCode != null)))
                {
                    products = await AddPriceAndExistFromHolooList(products);
                }

                var productIndexPageViewModel = new List<ProductIndexPageViewModel>();
                productIndexPageViewModel.AddRange(products.Select(product => (ProductIndexPageViewModel)product));

                switch (productListFilteredViewModel.ProductSort)
                {
                    case ProductSort.New:
                        productIndexPageViewModel = productIndexPageViewModel.OrderByDescending(x => x.Id).ToList();
                        break;
                    case ProductSort.Star:
                        productIndexPageViewModel = productIndexPageViewModel.OrderByDescending(x => x.Stars).ToList();
                        break;
                    case ProductSort.HighToLowPrice:
                        productIndexPageViewModel = productIndexPageViewModel.OrderByDescending(x => x.Price).ToList();
                        break;
                    case ProductSort.LowToHighPrice:
                        productIndexPageViewModel = productIndexPageViewModel.OrderBy(x => x.Price).ToList();
                        break;
                    case ProductSort.Bestsellers:
                        productIndexPageViewModel = productIndexPageViewModel.OrderBy(x => x.Price).ToList();
                        break;
                }

                var entity = PagedList<ProductIndexPageViewModel>.ToPagedList(productIndexPageViewModel,
                    productListFilteredViewModel.PaginationParameters.PageNumber,
                    productListFilteredViewModel.PaginationParameters.PageSize);

                var paginationDetails = new PaginationDetails
                {
                    TotalCount = entity.TotalCount,
                    PageSize = entity.PageSize,
                    CurrentPage = entity.CurrentPage,
                    TotalPages = entity.TotalPages,
                    HasNext = entity.HasNext,
                    HasPrevious = entity.HasPrevious,
                    Search = productListFilteredViewModel.PaginationParameters.Search
                };


                return Ok(new ApiResult
                {
                    PaginationDetails = paginationDetails,
                    Code = ResultCode.Success,
                    ReturnData = entity
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }

        [HttpGet]
        public async Task<IActionResult> TopDiscountsProducts(int count, CancellationToken cancellationToken)
        {
            try
            {
                var products = await _productRepository.TopDiscounts(count, cancellationToken);
                if (products.Any(x => x.Prices.Any(p => p.ArticleCode != null)))
                {
                    products = await AddPriceAndExistFromHolooList(products);
                }
                var productIndexPageViewModel = new List<ProductIndexPageViewModel>();
                productIndexPageViewModel.AddRange(products.Select(product => (ProductIndexPageViewModel)product));
                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = productIndexPageViewModel
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }

        [HttpGet]
        public async Task<IActionResult> TopRelativesProducts(int productId, int count, CancellationToken cancellationToken)
        {
            try
            {
                var products = await _productRepository.TopRelatives(productId, count, cancellationToken);
                if (products.Any(x => x.Prices.Any(p => p.ArticleCode != null)))
                {
                    products = await AddPriceAndExistFromHolooList(products);
                }
                var productIndexPageViewModel = new List<ProductIndexPageViewModel>();
                productIndexPageViewModel.AddRange(products.Select(product => (ProductIndexPageViewModel)product));
                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = productIndexPageViewModel
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }

        [HttpGet]
        public async Task<IActionResult> TopSellsProducts(int count, CancellationToken cancellationToken)
        {
            try
            {
                var products = await _productRepository.TopSells(count, cancellationToken);
                if (products.Any(x => x.Prices.Any(p => p.ArticleCode != null)))
                {
                    products = await AddPriceAndExistFromHolooList(products);
                }
                var productIndexPageViewModel = new List<ProductIndexPageViewModel>();
                productIndexPageViewModel.AddRange(products.Select(product => (ProductIndexPageViewModel)product));
                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = productIndexPageViewModel
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }

        [HttpGet]
        public async Task<IActionResult> TopStarsProducts(int count, CancellationToken cancellationToken)
        {
            try
            {
                var products = await _productRepository.TopStars(count, cancellationToken);
                if (products.Any(x => x.Prices.Any(p => p.ArticleCode != null)))
                {
                    products = await AddPriceAndExistFromHolooList(products);
                }
                var productIndexPageViewModel = new List<ProductIndexPageViewModel>();
                productIndexPageViewModel.AddRange(products.Select(product => (ProductIndexPageViewModel)product));
                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = productIndexPageViewModel
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }

        [HttpGet]
        public IActionResult GetCategoryProductCount(int categoryId, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = _productRepository.GetCategoryProductCount(categoryId, cancellationToken)
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }


        [HttpGet]
        public async Task<ActionResult<Product>> GetByProductUrl(string productUrl, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _productRepository.GetByUrl(productUrl, cancellationToken);
                if (result.Prices.Any(p => p.ArticleCode != null))
                {
                    result = await AddPriceAndExistFromHoloo(result);
                }
                var productIndexPageViewModel = new List<ProductIndexPageViewModel>();
                if (result == null)
                {
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.NotFound
                    });
                }

                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = result
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }

        [HttpGet]
        public async Task<ActionResult<ProductViewModel>> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _productRepository.GetProductByIdWithInclude(id, cancellationToken);
                if (result == null)
                {
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.NotFound
                    });
                }

                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = result
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProductsWithIdsForCompare(List<int> productIdList, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _productRepository.GetProductListWithAttribute(productIdList, cancellationToken);
                if (result == null)
                {
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.NotFound
                    });
                }

                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = result
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProductsWithIdsForCart(List<int> productIdList, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _productRepository.GetProductList(productIdList, cancellationToken);
                if (result == null)
                {
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.NotFound
                    });
                }

                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = result
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Post(ProductViewModel productViewModel, CancellationToken cancellationToken)
        {
            try
            {
                if (productViewModel == null)
                {
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.BadRequest
                    });
                }
                productViewModel.Name = productViewModel.Name.Trim();

                var repetitiveName = await _productRepository.GetByName(productViewModel.Name, cancellationToken);
                if (repetitiveName != null)
                {
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.Repetitive,
                        Messages = new List<string> { "نام محصول تکراری است" }
                    });
                }

                var repetitiveUrl = await _productRepository.GetByUrl(productViewModel.Url, cancellationToken);
                if (repetitiveUrl != null)
                {
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.Repetitive,
                        Messages = new List<string> { "آدرس محصول تکراری است" }
                    });
                }

                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = await _productRepository.AddWithRelations(productViewModel, cancellationToken)
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<int>> Put(ProductViewModel productViewModel, CancellationToken cancellationToken)
        {
            try
            {
                if (productViewModel == null)
                {
                    return BadRequest();
                }

                var repetitiveName = await _productRepository.GetByName(productViewModel.Name, cancellationToken);
                if (repetitiveName != null && repetitiveName.Id != productViewModel.Id)
                {
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.Repetitive,
                        Messages = new List<string> { "نام محصول تکراری است" }
                    });
                }
                if (repetitiveName != null) { _productRepository.Detach(repetitiveName); }

                var repetitiveUrl = await _productRepository.GetByUrl(productViewModel.Url, cancellationToken);
                if (repetitiveUrl != null && repetitiveUrl.Id != productViewModel.Id)
                {
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.Repetitive,
                        Messages = new List<string> { "آدرس محصول تکراری است" }
                    });
                }
                if (repetitiveUrl != null) { _productRepository.Detach(repetitiveUrl); }

                var result = await _productRepository.EditWithRelations(productViewModel, cancellationToken);
                if (productViewModel.Prices.Count > 0) { await _priceRepository.EditAll(productViewModel.Prices, result.Id, cancellationToken); }

                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = result
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }

        [HttpDelete]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _productRepository.DeleteAsync(id, cancellationToken);
                return Ok(new ApiResult
                {
                    Code = ResultCode.Success
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMGroup(CancellationToken cancellationToken)
        {
            try
            {
                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = await _mGroupRepository.GetAll(cancellationToken)
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSGroupByMGroupCode(string mCode, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _sGroupRepository.GetSGroupByMCode(mCode, cancellationToken);
                if (result == null)
                {
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.NotFound
                    });
                }

                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = result
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticleMCodeSCode(string code, CancellationToken cancellationToken)
        {
            try
            {
                //var products = (await _articleRepository.GetAllArticleMCodeSCode(code, cancellationToken))!.
                //    Select(x => new Product()
                //    {
                //        Name = x.A_Name,
                //        Description = x.Other1,
                //        MinInStore = x.A_Min,
                //        IsActive = x.IsActive,
                //        Prices = new List<Price>()
                //        {
                //            new Price()
                //            {
                //                Amount = Convert.ToInt32(x.Sel_Price),
                //                CurrencyId = 1,
                //                MinQuantity = 1,
                //                MaxQuantity = 0,
                //                UnitId =_unitRepository.GetId(x.VahedCode,cancellationToken),
                //                SellNumber=0,
                //                ArticleCode = x.A_Code,
                //                ArticleCodeCustomer = x.A_Code_C,
                //                Exist = x.Exist ?? 0
                //            }
                //        }
                //    });
                var products = await _articleRepository.GetAllArticleMCodeSCode(code, cancellationToken);
                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = products
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> ConvertHolooToSunflower([FromBody] string mCode, CancellationToken cancellationToken)
        {
            try
            {
                var mCodeLis = new List<HolooMGroup>();
                if (mCode.Equals(""))
                {
                    mCodeLis = (await _mGroupRepository.GetAll(cancellationToken))!.ToList();
                }
                else
                {
                    mCodeLis.Add(await _mGroupRepository.GetByCode(mCode, cancellationToken));
                }
                foreach (var mGroup in mCodeLis)
                {
                    Category parentCategoryCode = await _categoryRepository.GetByName(mGroup.M_groupname, cancellationToken) ?? await _categoryRepository.AddAsync(new Category()
                    {
                        Name = mGroup.M_groupname,
                        Path = "/" + mGroup.M_groupname
                    }, cancellationToken);

                    var sGroups = await _sGroupRepository.GetSGroupByMCode(mGroup.M_groupcode, cancellationToken);

                    var holooSGroups = sGroups.ToList();
                    if (!holooSGroups.Any())
                    {
                        if (mCode == "")
                            continue;
                        return Ok(new ApiResult
                        {
                            Code = ResultCode.BadRequest,
                            Messages = new List<string> { "گروه فرعی برای این گروه اصلی در هلو موجود نمی باشد" }
                        });
                    }
                    var categories = holooSGroups.Select(x => new Category()
                    {
                        Name = x.S_groupname,
                        ParentId = parentCategoryCode.Id,
                        Path = parentCategoryCode.Path + "/" + x.S_groupname
                    }).ToList();

                    foreach (var sGroup in holooSGroups)
                    {

                        var articles = await _articleRepository.GetAllArticleMCodeSCode(sGroup.M_groupcode + sGroup.S_groupcode, cancellationToken);
                        if (!articles.Any()) continue;
                        var category = await _categoryRepository.AddAsync(new Category()
                        {
                            Name = sGroup.S_groupname,
                            ParentId = parentCategoryCode.Id,
                            Path = parentCategoryCode.Path + "/" + sGroup.S_groupname
                        }, cancellationToken);
                        IEnumerable<Product> products = articles.
                            Select(x => new Product()
                            {
                                ProductCategories = new List<Category>() { category },
                                Name = x.A_Name,
                                Description = x.Other1,
                                MinInStore = x.A_Min,
                                IsActive = x.IsActive,
                                DiscountId = 1,
                                Url = x.A_Name.Replace(" ", "_"),
                                HolooCompanyId = 1,
                                SupplierId = 1,
                                StoreId = 1,
                                Prices = new List<Price>
                                {
                                    new Price
                                    {
                                        Amount = Convert.ToInt32(x.Sel_Price),
                                        CurrencyId = 1,
                                        MinQuantity = 1,
                                        MaxQuantity = 0,
                                        UnitId =_unitRepository.GetId(x.VahedCode,cancellationToken),
                                        ArticleCode = x.A_Code,
                                        ArticleCodeCustomer = x.A_Code_C,
                                        Exist = x.Exist ?? 0,
                                        SellNumber=0
                                    }
                                },
                                Images = new List<Image>
                                {
                                    new Image
                                    {
                                        Alt = "No Image",
                                        Path = "Images/Products",
                                        Name = "NoImage.png"
                                    }
                                }
                            });
                        try
                        {
                            await _productRepository.AddRangeAsync(products, cancellationToken);
                        }
                        catch (Exception e)
                        {
                            _logger.LogCritical(e, e.Message);
                            return Ok(new ApiResult { Code = ResultCode.Error, Messages = new List<string> { "افزودن اتوماتیک به مشکل برخورد کرد", e.Message } });
                        }
                    }

                    //var allHolooProducts = (await productRepository.GetAllHolooProducts())!.Select(x => new HolooArticle()
                    //{
                    //    A_Code = x.ArticleCode,
                    //    WebId = x.Id
                    //});
                    //result = await _articleRepository.SyncHolooWebId(allHolooProducts);
                    //if (result == 0)
                    //    return BadRequest("بروزرسانی هلو به مشکل برخورد کرد");
                }
                return Ok(new ApiResult
                {
                    Code = ResultCode.Success
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
            }
        }


    }
}