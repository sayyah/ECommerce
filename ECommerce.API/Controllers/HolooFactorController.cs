namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class HolooFactorController(IHolooFBailRepository fBailRepository, IHolooABailRepository aBailRepository,
        ILogger<FactorViewModel> logger)
    : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<IActionResult> Post(FactorViewModel factor, CancellationToken cancellationToken)
    {
        try
        {
            if (factor == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });

            factor.HolooFBail.Fac_Type = "P";
            var repetitiveBrand = await fBailRepository.Add(factor.HolooFBail, cancellationToken);
            if (repetitiveBrand != null)
            {
                for (var index = 0; index < factor.HolooABails.Count; index++)
                {
                    factor.HolooABails[index].Fac_Code = repetitiveBrand;
                    factor.HolooABails[index].Fac_Type = "P";
                }

                await aBailRepository.Add(factor.HolooABails, cancellationToken);
                return Ok(new ApiResult
                {
                    Code = ResultCode.Success
                });
            }

            return Ok(new ApiResult
            {
                Code = ResultCode.Error,
                Messages = new List<string> { "مشکل در ثبت فاکتور" }
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