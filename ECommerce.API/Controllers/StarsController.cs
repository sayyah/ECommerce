namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class StarsController(IUnitOfWork unitOfWork, ILogger<StarsController> logger) : ControllerBase
{
    private readonly IProductRepository _productRepository = unitOfWork.GetRepository<IProductRepository, Product>();
    private readonly IProductUserRankRepository _productUserRankRepository = unitOfWork.GetRepository<IProductUserRankRepository, ProductUserRank>();

    [HttpGet]
    public async Task<IActionResult> GetByProductId(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _productUserRankRepository.Where(x => x.ProductId == id, cancellationToken);
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
    public async Task<IActionResult> GetBySumProductId(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _productUserRankRepository.Where(x => x.ProductId == id, cancellationToken);
            if (result == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.NotFound
                });

            var average = await _productUserRankRepository.GetBySumProduct(id, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = average
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
    public async Task<IActionResult> GetByUserId(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _productUserRankRepository.Where(x => x.UserId == id, cancellationToken);
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
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<IActionResult> Post(ProductUserRank? productUserRank, CancellationToken cancellationToken)
    {
        try
        {
            if (productUserRank == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });

            if (productUserRank.Stars < 0)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest,
                    Messages = new List<string> { "مقدار وارد شده نادرست می‌باشد." }
                });

            var repetitiveProductUserRank = await _productUserRankRepository.GetByProductUser(productUserRank.ProductId,
                productUserRank.UserId, cancellationToken);
            if (repetitiveProductUserRank != null)
            {
                repetitiveProductUserRank.Stars = productUserRank.Stars;
                _productUserRankRepository.Update(repetitiveProductUserRank);
            }
            else
            {
                _productUserRankRepository.Add(productUserRank);
            }

            var productUserRanks =
                await _productUserRankRepository.GetBySumProduct(productUserRank.ProductId, cancellationToken);
            var product = await _productRepository.GetByIdAsync(cancellationToken, productUserRank.ProductId);
            if (product != null)
            {
                product.Star = productUserRanks;
                _productRepository.Update(product);
            }

            await unitOfWork.SaveAsync(cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = productUserRanks
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
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _productUserRankRepository.DeleteById(id, cancellationToken);
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