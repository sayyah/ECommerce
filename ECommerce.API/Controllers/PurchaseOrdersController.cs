using ECommerce.Application.Services.PurchaseOrders.Commands;
using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PurchaseOrdersController(IUnitOfWork unitOfWork, ILogger<PurchaseOrdersController> logger,
        IConfiguration configuration)
    : ControllerBase
{
    private readonly IHolooABailRepository _aBailRepository = unitOfWork.GetHolooRepository<HolooABailRepository, HolooABail>();
    private readonly IHolooArticleRepository _articleRepository = unitOfWork.GetHolooRepository<HolooArticleRepository, HolooArticle>();
    private readonly IDiscountRepository _discountRepository = unitOfWork.GetRepository<DiscountRepository, Discount>();
    private readonly IHolooCustomerRepository _customerRepository = unitOfWork.GetHolooRepository<HolooCustomerRepository, HolooCustomer>();
    private readonly IHolooFBailRepository _fBailRepository = unitOfWork.GetHolooRepository<HolooFBailRepository, HolooFBail>();
    private readonly IHolooSanadListRepository _sanadListRepository = unitOfWork.GetHolooRepository<HolooSanadListRepository, HolooSndList>();
    private readonly IHolooSanadRepository _sanadRepository = unitOfWork.GetHolooRepository<HolooSanadRepository, HolooSanad>();
    private readonly IPriceRepository _priceRepository = unitOfWork.GetRepository<PriceRepository, Price>();
    private readonly IProductRepository _productRepository = unitOfWork.GetRepository<ProductRepository, Product>();
    private readonly IPurchaseOrderDetailRepository _purchaseOrderDetailRepository = unitOfWork.GetRepository<PurchaseOrderDetailRepository, PurchaseOrderDetail>();
    private readonly IPurchaseOrderRepository _purchaseOrderRepository = unitOfWork.GetRepository<PurchaseOrderRepository, PurchaseOrder>();
    private readonly ITransactionRepository _transactionRepository = unitOfWork.GetRepository<TransactionRepository, Transaction>();
    private readonly IUserRepository _userRepository = unitOfWork.GetRepository<UserRepository, User>();

    private async Task<List<PurchaseOrderViewModel>> AddPriceAndExistFromHolooList(
        List<PurchaseOrderViewModel> products, CancellationToken cancellationToken)
    {
        int userCode = Convert.ToInt32(configuration.GetValue<string>("UserCode"));
        foreach (var product in products.Where(x => x.Price.ArticleCode != null))
            if (product.Price.SellNumber != null && product.Price.SellNumber != Price.HolooSellNumber.خالی)
            {
                var article = await _articleRepository.GetHolooPrice(product.Price.ArticleCodeCustomer!,
                    product.Price.SellNumber!.Value);
                product.PriceAmount = article.price / 10;
                double soldExist = article.a_Code.Sum(item => _aBailRepository.GetWithACode(userCode, item, cancellationToken));
                product.Exist = (double)article.exist! - soldExist;
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
                Code = ResultCode.DatabaseError,
                ReturnData = false,
                Messages = new List<string> { "اشکال در سمت سرور" }
            });
        purchaseOrder.Status = status;
        _purchaseOrderRepository.Update(purchaseOrder);
        await unitOfWork.SaveAsync(cancellationToken);

        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = true,
            Messages = new List<string> { "با موفقیت انجام شد" }
        });
    }


    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PurchaseFiltreOrderViewModel purchaseFilterOrderViewModel,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(purchaseFilterOrderViewModel.PaginationParameters?.Search))
                purchaseFilterOrderViewModel.PaginationParameters!.Search = "";
            var entity = await _purchaseOrderRepository.Search(purchaseFilterOrderViewModel, cancellationToken);
            var paginationDetails = new PaginationDetails
            {
                TotalCount = entity.TotalCount,
                PageSize = entity.PageSize,
                CurrentPage = entity.CurrentPage,
                TotalPages = entity.TotalPages,
                HasNext = entity.HasNext,
                HasPrevious = entity.HasPrevious,
                Search = purchaseFilterOrderViewModel.PaginationParameters.Search
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
            logger.LogCritical(e, e.Message);
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
            logger.LogCritical(e, e.Message);
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
            logger.LogCritical(e, e.Message);
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
            logger.LogCritical(e, e.Message);
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
            logger.LogCritical(e, e.Message);
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
            logger.LogCritical(e, e.Message);
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
            logger.LogCritical(e, e.Message);
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
            {
                result = await AddPriceAndExistFromHolooList(result.ToList(), cancellationToken);
            }

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
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<IActionResult> Post(CreatePurchaseCommand? createPurchaseCommand,
        CancellationToken cancellationToken)
    {
        try
        {
            if (createPurchaseCommand == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });

            var product = await _productRepository.GetByIdAsync(cancellationToken, createPurchaseCommand.ProductId);
            var prices = await _priceRepository.PriceOfProduct(createPurchaseCommand.ProductId, cancellationToken);
            if (prices != null)
            {
                var price = prices.FirstOrDefault(x => x.Id == createPurchaseCommand.PriceId);

                //var colleaguePrice = product.Prices.Where(x => x.IsColleague == createPurchaseCommand.IsColleague ).ToList();
                //var minPrice = colleaguePrice.Any()
                //    ? colleaguePrice.Where(x => x.MinQuantity <= createPurchaseCommand.Quantity).ToList()
                //    : product.Prices.Where(x => x.MinQuantity <= createPurchaseCommand.Quantity && x.ArticleCode == createPurchaseCommand.ArticleCode).ToList();
                //var maxPrice = minPrice.Any() ? minPrice.FirstOrDefault(x => x.MaxQuantity >= createPurchaseCommand.Quantity) :
                //    product.Prices.FirstOrDefault(x => x.MaxQuantity >= createPurchaseCommand.Quantity && x.ArticleCode == createPurchaseCommand.ArticleCode);

                //var price = maxPrice ?? product.Prices.FirstOrDefault();
                var repetitivePurchaseOrder =
                    await _purchaseOrderRepository.GetByUser(createPurchaseCommand.UserId, Status.New, cancellationToken);

                var repetitivePurchaseOrderDetails =
                    repetitivePurchaseOrder?.PurchaseOrderDetails?.FirstOrDefault(x =>
                        x.ProductId == createPurchaseCommand.ProductId);

                var repetitiveQuantity = repetitivePurchaseOrderDetails?.Quantity ?? 0;

                if (createPurchaseCommand.Quantity + repetitiveQuantity > product?.MaxOrder)
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.NotExist,
                        Messages = new[] { $"تعداد انتخابی کالا بیشتر از حد مجاز است. حد مجاز {product.MaxOrder} است" }
                    });

                if (product != null)
                {
                    product.MinInStore ??= 0;

                    decimal unitPrice;
                    if (price?.ArticleCode != null)
                    {
                        var userCode = Convert.ToInt32(configuration.GetValue<string>("UserCode"));
                        var soldExist = _aBailRepository.GetWithACode(userCode, price.ArticleCode, cancellationToken);
                        var holooPrice = await _articleRepository.GetHolooPrice(price.ArticleCodeCustomer!,
                            (Price.HolooSellNumber)price.SellNumber!);
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
                        if (repetitiveQuantity + createPurchaseCommand.Quantity > price?.Exist + product.MinInStore)
                            return Ok(new ApiResult
                            {
                                Code = ResultCode.NotExist,
                                Messages = new[] { "تعداد انتخابی کالا بیشتر از موجودی است" }
                            });
                        unitPrice = price?.Amount ?? 0;
                    }

                    var sumPrice = unitPrice * createPurchaseCommand.Quantity;

                    if (repetitivePurchaseOrder is { PurchaseOrderDetails: not null })
                    {
                        var repetitiveDetail =
                            repetitivePurchaseOrder.PurchaseOrderDetails.FirstOrDefault(x =>
                                x.ProductId == createPurchaseCommand.ProductId &&
                                x.PriceId == createPurchaseCommand.PriceId);
                        repetitivePurchaseOrder.Amount += sumPrice;

                        if (repetitiveDetail != null)
                        {
                            repetitiveDetail.Quantity += createPurchaseCommand.Quantity;
                            repetitiveDetail.SumPrice = repetitiveDetail.Quantity * unitPrice;
                            _purchaseOrderDetailRepository.Update(repetitiveDetail);
                            _purchaseOrderRepository.Update(repetitivePurchaseOrder);
                            await unitOfWork.SaveAsync(cancellationToken);

                            return Ok(new ApiResult
                            {
                                Code = ResultCode.Repetitive,
                                Messages = new[] { "کالا با موفقیت به سبد خرید اضافه شد" }
                            });
                        }

                        _purchaseOrderDetailRepository.Add(new PurchaseOrderDetail
                        {
                            PurchaseOrderId = repetitivePurchaseOrder.Id,
                            Name = product.Name,
                            UnitPrice = unitPrice,
                            ProductId = product.Id,
                            PriceId = price!.Id,
                            Quantity = createPurchaseCommand.Quantity,
                            SumPrice = sumPrice
                        });
                        _purchaseOrderRepository.Update(repetitivePurchaseOrder);
                        await unitOfWork.SaveAsync(cancellationToken);

                        return Ok(new ApiResult
                        {
                            Messages = new[] { "کالا با موفقیت به سبد خرید اضافه شد" },
                            Code = ResultCode.Success
                        });
                    }

                    var purchaseOrder = await _purchaseOrderRepository.AddAsync(new PurchaseOrder
                    {
                        Amount = sumPrice,
                        Status = 0,
                        UserId = createPurchaseCommand.UserId,
                        DiscountAmount = createPurchaseCommand.DiscountAmount
                    }, cancellationToken);
                    var purchaseOrderDetail = await _purchaseOrderDetailRepository.AddAsync(new PurchaseOrderDetail
                    {
                        PurchaseOrderId = purchaseOrder.Id,
                        Name = product.Name,
                        UnitPrice = unitPrice,
                        ProductId = product.Id,
                        PriceId = price!.Id,
                        Quantity = createPurchaseCommand.Quantity,
                        SumPrice = sumPrice
                    }, cancellationToken);
                    purchaseOrder.PurchaseOrderDetails?.Add(purchaseOrderDetail);
                }
            }

            await unitOfWork.SaveAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Messages = new[] { "کالا با موفقیت به سبد خرید اضافه شد" },
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> Decrease(PurchaseOrder? purchaseOrder, CancellationToken cancellationToken)
    {
        try
        {
            if (purchaseOrder != null)
            {
                var purchaseOrderDetails =
                    await _purchaseOrderDetailRepository.GetByIdAsync(cancellationToken, purchaseOrder.Id);
                if (purchaseOrderDetails != null)
                {
                    purchaseOrder =
                        await _purchaseOrderRepository.GetPurchaseOrderWithIncludeById(
                            (int)purchaseOrderDetails.PurchaseOrderId!, cancellationToken);
                    purchaseOrderDetails.Quantity -= 1;
                    if (purchaseOrderDetails.Quantity <= 0)
                    {
                        await _purchaseOrderDetailRepository.DeleteById(purchaseOrderDetails.Id, cancellationToken);
                    }
                    else
                    {
                        purchaseOrderDetails.SumPrice = purchaseOrderDetails.Quantity * purchaseOrderDetails.UnitPrice;
                        _purchaseOrderDetailRepository.Update(purchaseOrderDetails);
                    }

                    if (purchaseOrder != null)
                    {
                        purchaseOrder.Amount -= purchaseOrderDetails.UnitPrice;
                        if (purchaseOrder.Amount <= 0 || purchaseOrder.PurchaseOrderDetails == null)
                        {
                            await _purchaseOrderRepository.DeleteById(purchaseOrder.Id, cancellationToken);
                        }
                        else
                        {
                            purchaseOrder =
                                await _purchaseOrderRepository.GetPurchaseOrderWithIncludeById(
                                    (int)purchaseOrderDetails.PurchaseOrderId, cancellationToken);
                            if (purchaseOrder?.PurchaseOrderDetails != null)
                                purchaseOrder.Amount = purchaseOrder.PurchaseOrderDetails.Sum(x => x.SumPrice);
                            _purchaseOrderRepository.Update(purchaseOrder!);
                        }
                    }
                }
            }

            await unitOfWork.SaveAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
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
    public async Task<ActionResult<bool>> Pay(PurchaseOrder? purchaseOrder, CancellationToken cancellationToken)
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
                if (discount != null && (!discount.IsActive ||
                    discount.StartDate?.Date > DateTime.Now.Date ||
                    discount.EndDate?.Date < DateTime.Now.Date))
                    discount = null;
            }

            //purchaseOrder.PaymentDate = DateTime.Now;
            var resultUser = await _userRepository.GetByIdAsync(cancellationToken, purchaseOrder.UserId);
            if (resultUser != null)
            {
                var cCode = resultUser.CustomerCode;
                var (fCode, fCodeC) = await _fBailRepository.GetFactorCode(cancellationToken);
                var amount = Convert.ToDouble(purchaseOrder.Amount);
                double? calculateDiscount = null;
                if (discount != null) calculateDiscount = (amount - CalculateDiscount(discount, amount)) * 10;

                amount *= 10;
                var userCode = Convert.ToInt32(configuration.GetValue<string>("UserCode"));
                var fBail = await _fBailRepository.Add(new HolooFBail
                {
                    C_Code = cCode,
                    Fac_Code = fCode,
                    Fac_Code_C = fCodeC,
                    Fac_Comment =
                        $"پیش فاکتور از سایت برای سفارش شماره {purchaseOrder.OrderGuid} به آدرس : {purchaseOrder.SendInformation?.State?.Name} - {purchaseOrder.SendInformation?.City?.Name} - {purchaseOrder.SendInformation?.Address}, کد پستی : {purchaseOrder.SendInformation?.PostalCode}, شماره تماس : {purchaseOrder.SendInformation?.Mobile}",
                    Fac_Date = DateTime.Now,
                    Fac_Time = DateTime.Now,
                    Fac_Type = "P",
                    Sum_Price = amount,
                    Takhfif = calculateDiscount,
                    UserCode = userCode
                }, cancellationToken);

                var aBail = new List<HolooABail>();
                var i = 1;

                var purchaseOrderDetails =
                    await _purchaseOrderDetailRepository.GetByPurchaseOrderId(purchaseOrder.Id, cancellationToken);
                var holooArticle = await _articleRepository.GetHolooArticlesDefaultWarehouse(
                    purchaseOrderDetails.Select(c => c.Price?.ArticleCodeCustomer).ToList(), cancellationToken);
                foreach (var orderDetail in purchaseOrderDetails)
                {
                    var twoFactor = false;
                    double defaultWarehouseCount = orderDetail.Quantity;
                    double otherWarehouseCount = orderDetail.Quantity;
                    var holooA = holooArticle?.FirstOrDefault(c => c.A_Code_C == orderDetail.Price?.ArticleCodeCustomer);
                    if (holooA?.A_Code != null)
                    {
                        double soldExist = _aBailRepository.GetWithACode(userCode, holooA.A_Code, cancellationToken);
                        if ((holooA.Exist - soldExist) > 0)
                        {
                            if ((holooA.Exist - soldExist) < orderDetail.Quantity)
                            {
                                twoFactor = true;
                                defaultWarehouseCount = (double)holooA.Exist! - soldExist;
                                otherWarehouseCount = orderDetail.Quantity - defaultWarehouseCount;
                            }

                            aBail.Add(new HolooABail
                            {
                                A_Code = holooA.A_Code,
                                ACode_C = orderDetail.Price?.ArticleCodeCustomer,
                                A_Index = Convert.ToInt16(i++),
                                Fac_Code = fBail,
                                Fac_Type = "P",
                                Few_Article = defaultWarehouseCount,
                                First_Article = defaultWarehouseCount,
                                Price_BS = Convert.ToDouble(orderDetail.UnitPrice) * 10,
                                Unit_Few = 0
                            });
                        }

                        if (holooA.Exist - soldExist == 0 || twoFactor)
                        {
                            var holooArticleOther = await _articleRepository.GetHolooArticlesOtherWarehouse(
                                purchaseOrderDetails.Select(c => c.Price?.ArticleCodeCustomer).ToList(), cancellationToken);
                            var holooAOther = holooArticleOther.FirstOrDefault(c => c.A_Code_C == orderDetail.Price?.ArticleCodeCustomer);
                            double soldOtherExist = _aBailRepository.GetWithACode(userCode, holooAOther?.A_Code!, cancellationToken);
                            if (holooAOther?.Exist - soldOtherExist > 0)
                                aBail.Add(new HolooABail
                                {
                                    A_Code = holooAOther?.A_Code!,
                                    ACode_C = orderDetail.Price?.ArticleCodeCustomer,
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
                        }
                    }
                }


                _aBailRepository.Add(aBail);
                purchaseOrder.FBailCode = fBail;


                if (cCode != null)
                {
                    var customer = await _customerRepository.GetCustomerByCode(cCode);
                    var sanad = new HolooSanad($"کدرهگیری:{purchaseOrder.Transaction?.RefId}-{purchaseOrder.Description}");
                    var sanadCodes = await _sanadRepository.Add(sanad, cancellationToken);
                    var sanadCode = Convert.ToInt32(sanadCodes.Item1);
                    var sanadCodeCustomer = Convert.ToInt32(sanadCodes.Item2);
                    if (purchaseOrder.Transaction != null)
                    {
                        purchaseOrder.Transaction.Amount *= 10;
                        _transactionRepository.Add(new Transaction
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
                        });

                        var colCode = configuration.GetValue<string>("SiteSettings:SanadSettings:col_Code");
                        var moienCode = configuration.GetValue<string>("SiteSettings:SanadSettings:moien_Code");
                        var tafziliCode = configuration.GetValue<string>("SiteSettings:SanadSettings:tafzili_Code");

                        if (colCode != null && moienCode != null && tafziliCode != null && customer.Moien_Code_Bed != null)
                        {
                            _sanadListRepository.Add(
                                new HolooSndList(sanadCode, colCode, moienCode, tafziliCode,
                                    Convert.ToDouble(purchaseOrder.Transaction.Amount), 0,
                                    $"فاکتور شماره {fCodeC} سفارش در سایت به شماره {purchaseOrder.OrderGuid}"));
                            _sanadListRepository.Add(
                                new HolooSndList(sanadCode, "103", customer.Moien_Code_Bed, "", 0,
                                    Convert.ToDouble(purchaseOrder.Transaction.Amount),
                                    $"فاکتور شماره {fCodeC} سفارش در سایت به شماره {purchaseOrder.OrderGuid}"));
                        }
                    }
                }

                purchaseOrder.IsPaid = true;

                _purchaseOrderRepository.Update(purchaseOrder);
                await unitOfWork.SaveAsync(cancellationToken, true);

                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    Messages = new List<string> { fBail }
                });
            }
            return Ok(new ApiResult
            {
                Code = ResultCode.NotFound,
                Messages = new List<string> { "دسترسی غیر مجاز"}
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
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
            _purchaseOrderRepository.Update(purchaseOrder);
            await unitOfWork.SaveAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
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
            await _purchaseOrderDetailRepository.DeleteById(id, cancellationToken);
            if (purchaseOrderDetails != null)
            {
                var purchaseOrder =
                    await _purchaseOrderRepository.GetPurchaseOrderWithIncludeById(
                        (int)purchaseOrderDetails.PurchaseOrderId!, cancellationToken);
                if (purchaseOrder?.PurchaseOrderDetails != null)
                    purchaseOrder.Amount = purchaseOrder.PurchaseOrderDetails.Sum(x => x.SumPrice);
                if (purchaseOrder != null) _purchaseOrderRepository.Update(purchaseOrder);
            }

            await unitOfWork.SaveAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }
}