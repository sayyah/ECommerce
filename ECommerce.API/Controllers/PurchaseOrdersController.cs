﻿using ECommerce.Application.Services.Commands.Purchase.Purchases;
using ECommerce.Domain.Entities.HolooEntity;
<<<<<<< HEAD
using Microsoft.CodeAnalysis.CSharp.Syntax;
=======
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PurchaseOrdersController : ControllerBase
{
    private readonly IHolooABailRepository _aBailRepository;
    private readonly IHolooArticleRepository _articleRepository;
    private readonly IConfiguration _configuration;
    private readonly IDiscountRepository _discountRepository;
    private readonly IHolooABailRepository _holooABailRepository;
    private readonly IHolooCustomerRepository _holooCustomerRepository;
    private readonly IHolooFBailRepository _holooFBailRepository;
    private readonly IHolooSanadListRepository _holooSanadListRepository;
    private readonly IHolooSanadRepository _holooSanadRepository;
    private readonly ILogger<PurchaseOrdersController> _logger;
    private readonly IPriceRepository _priceRepository;
    private readonly IProductRepository _productRepository;
    private readonly IPurchaseOrderDetailRepository _purchaseOrderDetailRepository;
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;

    public PurchaseOrdersController(
        IPurchaseOrderRepository purchaseOrderRepository,
        IPurchaseOrderDetailRepository purchaseOrderDetailRepository,
        IProductRepository productRepository,
        ILogger<PurchaseOrdersController> logger,
        IHolooArticleRepository articleRepository,
        IPriceRepository priceRepository,
        IHolooFBailRepository holooFBailRepository,
        IHolooABailRepository holooABailRepository,
        IHolooSanadRepository holooSanadRepository,
        IHolooSanadListRepository holooSanadListRepository,
        IUserRepository userRepository,
        IHolooCustomerRepository holooCustomerRepository,
        ITransactionRepository transactionRepository,
        IDiscountRepository discountRepository,
        IHolooABailRepository aBailRepository,
        IConfiguration configuration)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _purchaseOrderDetailRepository = purchaseOrderDetailRepository;
        _productRepository = productRepository;
        _logger = logger;
        _articleRepository = articleRepository;
        _priceRepository = priceRepository;
        _holooFBailRepository = holooFBailRepository;
        _holooABailRepository = holooABailRepository;
        _holooSanadRepository = holooSanadRepository;
        _holooSanadListRepository = holooSanadListRepository;
        _userRepository = userRepository;
        _holooCustomerRepository = holooCustomerRepository;
        _transactionRepository = transactionRepository;
        _discountRepository = discountRepository;
        _configuration = configuration;
        _aBailRepository = aBailRepository;
    }

    private async Task<List<PurchaseOrderViewModel>> AddPriceAndExistFromHolooList(
        List<PurchaseOrderViewModel> products, CancellationToken cancellationToken)
    {
        int userCode = Convert.ToInt32(_configuration.GetValue<string>("UserCode"));
        foreach (var product in products.Where(x => x.Price.ArticleCode != null))
            if (product.Price.SellNumber != null && product.Price.SellNumber != Price.HolooSellNumber.خالی)
            {
                var article = await _articleRepository.GetHolooPrice(product.Price.ArticleCodeCustomer!,
                    product.Price.SellNumber!.Value);
                product.PriceAmount = article.price / 10;
                double soldExist = 0;
                foreach (var item in article.a_Code)
                    soldExist += _aBailRepository.GetWithACode(userCode, item, cancellationToken);
                product.Exist = (double)article.exist - soldExist;
                product.SumPrice = product.PriceAmount * product.Quantity;
            }

        return products;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> SetStatusById(int id, Status status, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(cancellationToken, id);
        if (purchaseOrder == null)
            return Ok(new ApiResult
            {
<<<<<<< HEAD
                Code = ResultCode.DatabaseError,
                ReturnData = false,
                Messages = new List<string> { "اشکال در سمت سرور" }
=======
                Code = ResultCode.DatabaseError, ReturnData = false, Messages = new List<string> { "اشکال در سمت سرور" }
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)
            });
        purchaseOrder.Status = status;
        var result = await _purchaseOrderRepository.UpdateAsync(purchaseOrder, cancellationToken);
        if (result == null)
            return Ok(new ApiResult
            {
<<<<<<< HEAD
                Code = ResultCode.DatabaseError,
                ReturnData = false,
                Messages = new List<string> { "اشکال در سمت سرور" }
=======
                Code = ResultCode.DatabaseError, ReturnData = false, Messages = new List<string> { "اشکال در سمت سرور" }
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)
            });

        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = true,
            Messages = new List<string> { "با موفقیت انجام شد" }
        });
    }


    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PurchaseFiltreOrderViewModel purchaseFiltreOrderViewModel,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(purchaseFiltreOrderViewModel.PaginationParameters.Search))
                purchaseFiltreOrderViewModel.PaginationParameters.Search = "";
            var entity = await _purchaseOrderRepository.Search(purchaseFiltreOrderViewModel, cancellationToken);
            var paginationDetails = new PaginationDetails
            {
                TotalCount = entity.TotalCount,
                PageSize = entity.PageSize,
                CurrentPage = entity.CurrentPage,
                TotalPages = entity.TotalPages,
                HasNext = entity.HasNext,
                HasPrevious = entity.HasPrevious,
                Search = purchaseFiltreOrderViewModel.PaginationParameters.Search
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
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpGet]
    public async Task<ActionResult<PurchaseOrder>> GetByUserAndOrderId(int userId, long orderId,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _purchaseOrderRepository.GetByUserAndOrderId(userId, orderId, cancellationToken);
            if (result == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.NotFound
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = result
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<PurchaseOrder>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _purchaseOrderRepository.GetByIdAsync(cancellationToken, id);
            if (result == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.NotFound
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = result
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<List<PurchaseOrder>>> GetPurchaseOrderWithIncludeById(int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _purchaseOrderRepository.GetPurchaseOrderWithIncludeById(id, cancellationToken);
            if (result == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.NotFound
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = result
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<PurchaseOrder>> GetByUserId(int userId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _purchaseOrderRepository.GetByUser(userId, Status.New, cancellationToken);
            if (result == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.NotFound
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = result
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<PurchaseOrder>> GetByOrderId(long orderId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _purchaseOrderRepository.GetByOrderId(orderId, cancellationToken);
            if (result == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.NotFound
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = result
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    //[Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<PurchaseOrder>> GetByOrderIdWithInclude(long orderId,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _purchaseOrderRepository.GetByOrderIdWithInclude(orderId, cancellationToken);
            if (result == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.NotFound
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = result
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<PurchaseOrderViewModel>> UserCart(int userId, bool shouldUpdatePurchaseOrderDetails,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _purchaseOrderRepository.GetProductListByUserId(userId, cancellationToken);

            if (result == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = new List<PurchaseOrderViewModel>()
                });

            if (result.Any(x => x.Price.ArticleCode != null))
<<<<<<< HEAD
            {
                result = await AddPriceAndExistFromHolooList(result.ToList(), cancellationToken);
            }
=======
                result = await AddPriceAndExistFromHolooList(result.ToList());
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

            if (shouldUpdatePurchaseOrderDetails)
                await _purchaseOrderDetailRepository.UpdateUserCart(result, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = result
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<IActionResult> Post(CreatePurchaseCommand createPurchaseCommand,
        CancellationToken cancellationToken)
    {
        try
        {
            if (createPurchaseCommand == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });

            var purchaseOrder = new PurchaseOrder();
            var purchaseOrderDetail = new PurchaseOrderDetail();
            var product = await _productRepository.GetByIdAsync(cancellationToken, createPurchaseCommand.ProductId);
            var prices = await _priceRepository.PriceOfProduct(createPurchaseCommand.ProductId, cancellationToken);
            var price = prices.FirstOrDefault(x => x.Id == createPurchaseCommand.PriceId);

            //var colleaguePrice = product.Prices.Where(x => x.IsColleague == createPurchaseCommand.IsColleague ).ToList();
            //var minPrice = colleaguePrice.Any()
            //    ? colleaguePrice.Where(x => x.MinQuantity <= createPurchaseCommand.Quantity).ToList()
            //    : product.Prices.Where(x => x.MinQuantity <= createPurchaseCommand.Quantity && x.ArticleCode == createPurchaseCommand.ArticleCode).ToList();
            //var maxPrice = minPrice.Any() ? minPrice.FirstOrDefault(x => x.MaxQuantity >= createPurchaseCommand.Quantity) :
            //    product.Prices.FirstOrDefault(x => x.MaxQuantity >= createPurchaseCommand.Quantity && x.ArticleCode == createPurchaseCommand.ArticleCode);

            //var price = maxPrice ?? product.Prices.FirstOrDefault();
            decimal unitPrice = 0;
            var repetitivePurchaseOrder =
                await _purchaseOrderRepository.GetByUser(createPurchaseCommand.UserId, Status.New, cancellationToken);

            var repetitivePurchaseOrderDetails =
                repetitivePurchaseOrder?.PurchaseOrderDetails?.FirstOrDefault(x =>
                    x.ProductId == createPurchaseCommand.ProductId);

            var repetitiveQuantity = repetitivePurchaseOrderDetails?.Quantity ?? 0;

            if (createPurchaseCommand.Quantity + repetitiveQuantity > product.MaxOrder)
                return Ok(new ApiResult
                {
                    Code = ResultCode.NotExist,
                    Messages = new[] { $"تعداد انتخابی کالا بیشتر از حد مجاز است. حد مجاز {product.MaxOrder} است" }
                });

            if (product.MinInStore == null) product.MinInStore = 0;

            double soldExist = 0;
            if (price.ArticleCode != null)
            {
                var userCode = Convert.ToInt32(_configuration.GetValue<string>("UserCode"));
                soldExist = _aBailRepository.GetWithACode(userCode, price.ArticleCode, cancellationToken);
                var holooPrice = await _articleRepository.GetHolooPrice(price.ArticleCodeCustomer,
                    (Price.HolooSellNumber)price.SellNumber);
                if (repetitiveQuantity + createPurchaseCommand.Quantity >
                    holooPrice.exist + product.MinInStore - soldExist)
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.NotExist,
                        Messages = new[] { "تعداد انتخابی کالا بیشتر از موجودی است" }
                    });
                unitPrice = holooPrice.price;
            }
            else
            {
                if (repetitiveQuantity + createPurchaseCommand.Quantity > price.Exist + product.MinInStore)
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.NotExist,
                        Messages = new[] { "تعداد انتخابی کالا بیشتر از موجودی است" }
                    });
                unitPrice = price?.Amount ?? 0;
            }

            var sumPrice = unitPrice * createPurchaseCommand.Quantity;

            if (repetitivePurchaseOrder != null)
            {
                var repetitiveDetail =
                    repetitivePurchaseOrder.PurchaseOrderDetails.FirstOrDefault(x =>
                        x.ProductId == createPurchaseCommand.ProductId && x.PriceId == createPurchaseCommand.PriceId);
                repetitivePurchaseOrder.Amount += sumPrice;

                if (repetitiveDetail != null)
                {
                    repetitiveDetail.Quantity += createPurchaseCommand.Quantity;
                    repetitiveDetail.SumPrice = repetitiveDetail.Quantity * unitPrice;
                    await _purchaseOrderDetailRepository.UpdateAsync(repetitiveDetail, cancellationToken);
                    await _purchaseOrderRepository.UpdateAsync(repetitivePurchaseOrder, cancellationToken);
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.Repetitive,
                        Messages = new[] { "کالا با موفقیت به سبد خرید اضافه شد" }
                    });
                }

                await _purchaseOrderDetailRepository.AddAsync(new PurchaseOrderDetail
                {
                    PurchaseOrderId = repetitivePurchaseOrder.Id,
                    Name = product.Name,
                    UnitPrice = unitPrice,
                    ProductId = product.Id,
                    PriceId = price!.Id,
                    Quantity = createPurchaseCommand.Quantity,
                    SumPrice = sumPrice
                }, cancellationToken);
                await _purchaseOrderRepository.UpdateAsync(repetitivePurchaseOrder, cancellationToken);

                return Ok(new ApiResult
                {
                    Messages = new[] { "کالا با موفقیت به سبد خرید اضافه شد" },
                    Code = ResultCode.Success
                });
            }

            purchaseOrder = await _purchaseOrderRepository.AddAsync(new PurchaseOrder
            {
                Amount = sumPrice,
                Status = 0,
                UserId = createPurchaseCommand.UserId,
                DiscountAmount = createPurchaseCommand.DiscountAmount
            }, cancellationToken);
            purchaseOrderDetail = await _purchaseOrderDetailRepository.AddAsync(new PurchaseOrderDetail
            {
                PurchaseOrderId = purchaseOrder.Id,
                Name = product.Name,
                UnitPrice = unitPrice,
                ProductId = product.Id,
                PriceId = price!.Id,
                Quantity = createPurchaseCommand.Quantity,
                SumPrice = sumPrice
            }, cancellationToken);
            purchaseOrder.PurchaseOrderDetails.Add(purchaseOrderDetail);
            return Ok(new ApiResult
            {
                Messages = new[] { "کالا با موفقیت به سبد خرید اضافه شد" },
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> Decrease(PurchaseOrder purchaseOrder, CancellationToken cancellationToken)
    {
        try
        {
            var purchaseOrderDetails =
                await _purchaseOrderDetailRepository.GetByIdAsync(cancellationToken, purchaseOrder.Id);
            purchaseOrder =
                await _purchaseOrderRepository.GetPurchaseOrderWithIncludeById(
                    (int)purchaseOrderDetails.PurchaseOrderId, cancellationToken);
            purchaseOrderDetails.Quantity -= 1;
            if (purchaseOrderDetails.Quantity <= 0)
            {
                await _purchaseOrderDetailRepository.DeleteAsync(purchaseOrderDetails.Id, cancellationToken);
            }
            else
            {
                purchaseOrderDetails.SumPrice = purchaseOrderDetails.Quantity * purchaseOrderDetails.UnitPrice;
                await _purchaseOrderDetailRepository.UpdateAsync(purchaseOrderDetails, cancellationToken);
            }

            purchaseOrder.Amount -= purchaseOrderDetails.UnitPrice;
            if (purchaseOrder.Amount <= 0 || purchaseOrder.PurchaseOrderDetails == null)
            {
                await _purchaseOrderRepository.DeleteAsync(purchaseOrder.Id, cancellationToken);
            }
            else
            {
                purchaseOrder =
                    await _purchaseOrderRepository.GetPurchaseOrderWithIncludeById(
                        (int)purchaseOrderDetails.PurchaseOrderId, cancellationToken);
                purchaseOrder.Amount = purchaseOrder.PurchaseOrderDetails.Sum(x => x.SumPrice);
                await _purchaseOrderRepository.UpdateAsync(purchaseOrder, cancellationToken);
            }

            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    private double CalculateDiscount(Discount discount, double amount)
    {
        if (discount.Amount is > 0)
        {
            amount -= (int)discount.Amount;
            if (amount < 0) amount = 0;
        }
        else
        {
            if (discount.Percent != null) amount -= discount.Percent.Value / 100 * amount;
        }

        return amount;
    }

    [HttpPut]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> Pay(PurchaseOrder purchaseOrder, CancellationToken cancellationToken)
    {
        try
        {
            if (purchaseOrder == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });
            if (string.IsNullOrEmpty(purchaseOrder.Description)) purchaseOrder.Description = "";
            Discount? discount = null;
            if (purchaseOrder.DiscountId != null)
            {
                discount = await _discountRepository.GetByIdAsync(cancellationToken, purchaseOrder.DiscountId);
                if (!discount.IsActive ||
                    discount.StartDate?.Date > DateTime.Now.Date ||
                    discount.EndDate?.Date < DateTime.Now.Date)
                    discount = null;
            }

            //purchaseOrder.PaymentDate = DateTime.Now;
            var resultUser = await _userRepository.GetByIdAsync(cancellationToken, purchaseOrder.UserId);
            var cCode = resultUser.CustomerCode;
            var (fCode, fCodeC) = await _holooFBailRepository.GetFactorCode(cancellationToken);
            var amount = Convert.ToDouble(purchaseOrder.Amount);
            double? takhfif = null;
            if (discount != null) takhfif = (amount - CalculateDiscount(discount, amount)) * 10;

            amount *= 10;
            var userCode = Convert.ToInt32(_configuration.GetValue<string>("UserCode"));
            var fBail = await _holooFBailRepository.Add(new HolooFBail
            {
                C_Code = cCode,
                Fac_Code = fCode,
                Fac_Code_C = fCodeC,
                Fac_Comment =
                    $"پیش فاکتور از سایت برای سفارش شماره {purchaseOrder.OrderGuid} به آدرس : {purchaseOrder.SendInformation.State.Name} - {purchaseOrder.SendInformation.City.Name} - {purchaseOrder.SendInformation.Address}, کد پستی : {purchaseOrder.SendInformation.PostalCode}, شماره تماس : {purchaseOrder.SendInformation.Mobile}",
                Fac_Date = DateTime.Now,
                Fac_Time = DateTime.Now,
                Fac_Type = "P",
                Sum_Price = amount,
                Takhfif = takhfif,
                UserCode = userCode
            }, cancellationToken);

            var aBail = new List<HolooABail>();
            var i = 1;

            var purchaseOrderDetails =
                await _purchaseOrderDetailRepository.GetByPurchaseOrderId(purchaseOrder.Id, cancellationToken);
            var holooArticle = await _articleRepository.GetHolooArticlesDefaultWarehouse(
                purchaseOrderDetails.Select(c => c.Price.ArticleCodeCustomer).ToList(), cancellationToken);
            foreach (var orderDetail in purchaseOrderDetails)
            {
                var twoFactor = false;
                double defaultWarehouseCount = orderDetail.Quantity;
                double otherWarehouseCount = orderDetail.Quantity;
                var holoo_A = holooArticle.Where(c => c.A_Code_C == orderDetail.Price.ArticleCodeCustomer)
                    .FirstOrDefault();
<<<<<<< HEAD
                double soldExist = _holooABailRepository.GetWithACode(userCode, holoo_A.A_Code, cancellationToken);
                if ((holoo_A.Exist - soldExist) > 0)
=======
                if (holoo_A.Exist > 0)
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)
                {
                    if ((holoo_A.Exist - soldExist) < orderDetail.Quantity)
                    {
                        twoFactor = true;
                        defaultWarehouseCount = (double)holoo_A.Exist - soldExist;
                        otherWarehouseCount = orderDetail.Quantity - defaultWarehouseCount;
                    }

                    aBail.Add(new HolooABail
                    {
                        A_Code = holoo_A.A_Code,
                        ACode_C = orderDetail.Price.ArticleCodeCustomer,
                        A_Index = Convert.ToInt16(i++),
                        Fac_Code = fBail,
                        Fac_Type = "P",
                        Few_Article = defaultWarehouseCount,
                        First_Article = defaultWarehouseCount,
                        Price_BS = Convert.ToDouble(orderDetail.UnitPrice) * 10,
                        Unit_Few = 0
                    });
                }

<<<<<<< HEAD
                if (holoo_A.Exist - soldExist == 0 || twoFactor)
=======
                if (holoo_A.Exist == 0 || twoFactor)
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)
                {
                    var holooArticleOthere = await _articleRepository.GetHolooArticlesOthereWarehouse(
                        purchaseOrderDetails.Select(c => c.Price.ArticleCodeCustomer).ToList(), cancellationToken);
                    var holoo_A_Othere = holooArticleOthere
                        .Where(c => c.A_Code_C == orderDetail.Price.ArticleCodeCustomer).FirstOrDefault();
<<<<<<< HEAD
                    double soldOtherExist = _holooABailRepository.GetWithACode(userCode, holoo_A_Othere.A_Code, cancellationToken);
                    if (holoo_A_Othere.Exist - soldOtherExist > 0)
                        aBail.Add(new HolooABail
                        {
                            A_Code = holoo_A_Othere.A_Code,
                            ACode_C = orderDetail.Price.ArticleCodeCustomer,
                            A_Index = Convert.ToInt16(i++),
                            Fac_Code = fBail,
                            Fac_Type = "P",
                            Few_Article = otherWarehouseCount,
                            First_Article = otherWarehouseCount,
                            Price_BS = Convert.ToDouble(orderDetail.UnitPrice) * 10,
                            Unit_Few = 0
                        });
                    else
                    {
                        return Ok(new ApiResult
                        {
                            Code = ResultCode.BadRequest,
                            Messages = new List<string> { "عدم تطابق موجودی کالا" }
                        });
                    }
=======
                    aBail.Add(new HolooABail
                    {
                        A_Code = holoo_A_Othere.A_Code,
                        ACode_C = orderDetail.Price.ArticleCodeCustomer,
                        A_Index = Convert.ToInt16(i++),
                        Fac_Code = fBail,
                        Fac_Type = "P",
                        Few_Article = otherWarehouseCount,
                        First_Article = otherWarehouseCount,
                        Price_BS = Convert.ToDouble(orderDetail.UnitPrice) * 10,
                        Unit_Few = 0
                    });
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)
                }
            }


            await _holooABailRepository.Add(aBail, cancellationToken);
            purchaseOrder.FBailCode = fBail;


            var customer = await _holooCustomerRepository.GetCustomerByCode(cCode);
            var sanad = new HolooSanad($"کدرهگیری:{purchaseOrder.Transaction.RefId}-{purchaseOrder.Description}");
            var sanadCodes = await _holooSanadRepository.Add(sanad, cancellationToken);
            var sanadCode = Convert.ToInt32(sanadCodes.Item1);
            var sanadCodeCustomer = Convert.ToInt32(sanadCodes.Item2);
            purchaseOrder.Transaction.Amount *= 10;
            await _transactionRepository.AddAsync(new Transaction
            {
                Amount = purchaseOrder.Transaction.Amount,
                PaymentId = purchaseOrder.Transaction.PaymentId,
                HolooCompanyId = purchaseOrder.Transaction.HolooCompanyId,
                PaymentMethodId = purchaseOrder.Transaction.PaymentMethodId,
                RefId = purchaseOrder.Transaction.RefId,
                UserId = purchaseOrder.Transaction.UserId,
                TransactionDate = DateTime.Now,
                SanadCode = sanadCode,
                SanadCodeCustomer = sanadCodeCustomer,
                PurchaseOrderId = purchaseOrder.Id
            }, cancellationToken);

            var col_Code = _configuration.GetValue<string>("SiteSettings:SanadSettings:col_Code");
            var moien_Code = _configuration.GetValue<string>("SiteSettings:SanadSettings:moien_Code");
            var tafzili_Code = _configuration.GetValue<string>("SiteSettings:SanadSettings:tafzili_Code");

            await _holooSanadListRepository.Add(
                new HolooSndList(sanadCode, col_Code, moien_Code, tafzili_Code,
                    Convert.ToDouble(purchaseOrder.Transaction.Amount), 0,
                    $"فاکتور شماره {fCodeC} سفارش در سایت به شماره {purchaseOrder.OrderGuid}"), cancellationToken);
            await _holooSanadListRepository.Add(
                new HolooSndList(sanadCode, "103", customer.Moien_Code_Bed, "", 0,
                    Convert.ToDouble(purchaseOrder.Transaction.Amount),
                    $"فاکتور شماره {fCodeC} سفارش در سایت به شماره {purchaseOrder.OrderGuid}"), cancellationToken);

            purchaseOrder.IsPaid = true;

            await _purchaseOrderRepository.UpdateAsync(purchaseOrder, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                Messages = new List<string> { fBail }
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
            {
                Code = ResultCode.DatabaseError
            });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> Put(PurchaseOrder purchaseOrder, CancellationToken cancellationToken)
    {
        try
        {
            await _purchaseOrderRepository.UpdateAsync(purchaseOrder, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpDelete]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            var purchaseOrderDetails = await _purchaseOrderDetailRepository.GetByIdAsync(cancellationToken, id);
            await _purchaseOrderDetailRepository.DeleteAsync(id, cancellationToken);
            var purchaseOrder =
                await _purchaseOrderRepository.GetPurchaseOrderWithIncludeById(
                    (int)purchaseOrderDetails.PurchaseOrderId, cancellationToken);
            purchaseOrder.Amount = purchaseOrder.PurchaseOrderDetails.Sum(x => x.SumPrice);
            await _purchaseOrderRepository.UpdateAsync(purchaseOrder, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }
}