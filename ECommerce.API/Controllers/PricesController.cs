using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PricesController(IUnitOfWork unitOfWork, ILogger<PricesController> logger) : ControllerBase
{
    private readonly IHolooArticleRepository _holooArticleRepository = unitOfWork.GetHolooRepository<IHolooArticleRepository, HolooArticle>();
    private readonly IPriceRepository _priceRepository = unitOfWork.GetRepository<IPriceRepository, Price>();

    /// <summary>
    ///     Get All Price By Product Id with Pagination.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity = await _priceRepository.Search(paginationParameters, cancellationToken);
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
    public async Task<ActionResult<Price>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _priceRepository.GetByIdAsync(cancellationToken, id);
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
    public async Task<IActionResult> GetProductsPriceById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _priceRepository.PriceOfProduct(id, cancellationToken);
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

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Post(Price? price, CancellationToken cancellationToken)
    {
        try
        {
            var messages = new List<string>();
            if (price == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });

            if (price.ArticleCode == null && price.Amount == 0)
                messages.Add("لطفا یا کد کالا وارد کنید یا مبلغ");

            messages.AddRange(await CheckPrice(price, cancellationToken));
            if (price is { ArticleCodeCustomer: not null, SellNumber: not null })
            {
                var holooPrice =
                    await _holooArticleRepository.GetHolooPrice(price.ArticleCodeCustomer, price.SellNumber.Value);
                if (holooPrice.price == 0)
                    messages.Add("شماره قیمت انتخاب شده فاقد مقدار می باشد");
            }

            if (messages.Count > 0)
                return Ok(new ApiResult
                {
                    Messages = messages,
                    Code = ResultCode.BadRequest
                });

            var newPrice = await _priceRepository.AddAsync(price, cancellationToken);
            _holooArticleRepository.SyncHolooWebId(newPrice.ArticleCodeCustomer!, newPrice.ProductId);
            await unitOfWork.SaveAsync(cancellationToken,true);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = newPrice
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
    public async Task<ActionResult<bool>> Put(Price price, CancellationToken cancellationToken)
    {
        try
        {
            var messages = new List<string>();
            if (price.ArticleCode == null && price.Amount == 0)
                messages.Add("لطفا یا کد کالا وارد کنید یا مبلغ");

            messages.AddRange(await CheckPrice(price, cancellationToken));

            if (price is { ArticleCodeCustomer: not null, SellNumber: not null })
            {
                var holooPrice =
                    await _holooArticleRepository.GetHolooPrice(price.ArticleCodeCustomer, price.SellNumber.Value);

                if (holooPrice.price == 0)
                    messages.Add("شماره قیمت انتخاب شده فاقد مقدار می باشد");
            }

            if (messages.Count > 0)
                return Ok(new ApiResult
                {
                    Messages = messages,
                    Code = ResultCode.BadRequest
                });

            _priceRepository.Update(price);
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
            await _priceRepository.DeleteById(id, cancellationToken);
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

    private async Task<List<string>> CheckPrice(Price price, CancellationToken cancellationToken)
    {
        var messages = new List<string>();
        var prices = await _priceRepository.PriceOfProduct(price.ProductId, cancellationToken);
        if (prices == null) return messages;
        var repetitive = prices.Where(x => x.Amount == price.Amount
                                           && x.IsColleague == price.IsColleague
                                           && x.ColorId == price.ColorId
                                           && x.SizeId == price.SizeId).ToList();
        if (repetitive.Any() && repetitive.All(x => x.Id != price.Id)) messages.Add("مبلغ وارد شده تکراری است");

        if (prices.Any(x => x.MinQuantity <= price.MinQuantity
                            && x.MaxQuantity >= price.MinQuantity
                            && x.IsColleague == price.IsColleague
                            && x.ColorId == price.ColorId
                            && x.SizeId == price.SizeId))
            messages.Add("حداقل تعداد در بازه تعداد های قبلی این کالا است");
        if (prices.Any(x => x.MinQuantity <= price.MaxQuantity
                            && x.MaxQuantity >= price.MaxQuantity
                            && x.IsColleague == price.IsColleague
                            && x.ColorId == price.ColorId
                            && x.SizeId == price.SizeId))
            messages.Add("حداکثر تعداد در بازه تعداد های قبلی این کالا است");

        return messages;
    }
}