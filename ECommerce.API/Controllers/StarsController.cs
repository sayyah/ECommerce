namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class StarsController(IProductUserRankRepository productUserRankRepository, ILogger<StarsController> logger,
        IProductRepository productRepository)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetByProductId(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await productUserRankRepository.Where(x => x.ProductId == id, cancellationToken);
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
            var result = await productUserRankRepository.Where(x => x.ProductId == id, cancellationToken);
            if (result == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.NotFound
                });

            var average = await productUserRankRepository.GetBySumProduct(id, cancellationToken);
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
            var result = await productUserRankRepository.Where(x => x.UserId == id, cancellationToken);
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
    public async Task<IActionResult> Post(ProductUserRank productUserRank, CancellationToken cancellationToken)
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

            var repetitiveProductUserRank = await productUserRankRepository.GetByProductUser(productUserRank.ProductId,
                productUserRank.UserId, cancellationToken);
            if (repetitiveProductUserRank != null)
            {
                repetitiveProductUserRank.Stars = productUserRank.Stars;
                await productUserRankRepository.UpdateAsync(repetitiveProductUserRank, cancellationToken);
            }
            else
            {
                await productUserRankRepository.AddAsync(productUserRank, cancellationToken);
            }

            var productUserRanks =
                await productUserRankRepository.GetBySumProduct(productUserRank.ProductId, cancellationToken);
            var product = await productRepository.GetByIdAsync(cancellationToken, productUserRank.ProductId);
            product.Star = productUserRanks;
            await productRepository.UpdateAsync(product, cancellationToken);
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
            await productUserRankRepository.DeleteAsync(id, cancellationToken);
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