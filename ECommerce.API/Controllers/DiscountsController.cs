using ECommerce.Domain.Entities.HolooEntity;
using Ecommerce.Entities.ViewModel;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class DiscountsController(IUnitOfWork unitOfWork, ILogger<DiscountsController> logger)
    : ControllerBase
{
    private readonly IHolooArticleRepository _articleRepository = unitOfWork.GetHolooRepository<HolooArticleRepository, HolooArticle>();
    private readonly IDiscountRepository _discountRepository = unitOfWork.GetRepository<DiscountRepository, Discount>();

    [HttpGet]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity = await _discountRepository.Search(paginationParameters, cancellationToken);
            var paginationDetails = new PaginationDetails
            {
                TotalCount = entity.TotalCount,
                PageSize = entity.PageSize,
                CurrentPage = entity.CurrentPage,
                TotalPages = entity.TotalPages,
                HasNext = entity.HasNext,
                HasPrevious = entity.HasPrevious,
                Search = paginationParameters.Search
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
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<Discount>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _discountRepository.GetByIdAsync(id, cancellationToken);

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
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<Discount>> GetByCode(string code, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _discountRepository.GetByCode(code, cancellationToken);
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
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<Discount>> GetLast(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _discountRepository.GetLast(cancellationToken);
            if (result == null && result?.Prices == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.NotFound
                });

            foreach (var productPrices in result.Prices!)
                if (productPrices.SellNumber != null && productPrices.SellNumber != Price.HolooSellNumber.خالی &&
                    productPrices is { ArticleCode: not null, ArticleCodeCustomer: not null })
                {
                    var article = await _articleRepository.GetHolooPrice(productPrices.ArticleCodeCustomer,
                        productPrices.SellNumber!.Value);

                    productPrices.Amount = article.price / 10;
                    productPrices.Exist = article.exist ?? 0;
                }

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = result
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
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<Discount>> GetWithTime(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _discountRepository.GetWithTime(cancellationToken);
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
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> ActiveDiscount(int id,CancellationToken cancellationToken)
    {
        try
        {
            var result = _discountRepository.Active(id);
            await unitOfWork.SaveAsync(cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = result
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Post(DiscountViewModel discount, CancellationToken cancellationToken)
    {
        try
        {
            if (discount == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });
            discount.Name = discount.Name.Trim();

            var repetitiveName = await _discountRepository.GetByName(discount.Name, cancellationToken);
            if (repetitiveName != null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "نام تخفیف تکراری است" }
                });

            var repetitiveCode = await _discountRepository.GetByCode(discount.Code, cancellationToken);
            if (repetitiveCode != null && discount.DiscountType==DiscountType.Coupon)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "کد تخفیف تکراری است" }
                });

            _discountRepository.Add(discount);
            await unitOfWork.SaveAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await _discountRepository.AddWithRelations(discount, cancellationToken)
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> Put(Discount discount, CancellationToken cancellationToken)
    {
        try
        {
            var repetitiveCode = await _discountRepository.GetByCode(discount.Code, cancellationToken);
            if (repetitiveCode != null && repetitiveCode.Id != discount.Id)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "کد تخفیف تکراری است" }
                });
            if (repetitiveCode != null && repetitiveCode.Id == discount.Id)
            {
                _discountRepository.Detach(repetitiveCode);
            }

            var repetitiveName = await _discountRepository.GetByName(discount.Name, cancellationToken);
            if (repetitiveName != null && repetitiveCode?.Id != discount.Id)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "نام تخفیف تکراری است" }
                });
            if (repetitiveName != null && repetitiveName.Id == discount.Id)
            {
                _discountRepository.Detach(repetitiveName);
            }

            _discountRepository.Update(discount);
            await unitOfWork.SaveAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpDelete]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _discountRepository.DeleteById(id, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }
}