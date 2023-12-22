using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class HolooFactorController(IUnitOfWork unitOfWork,
        ILogger<FactorViewModel> logger)
    : ControllerBase
{
    private readonly IHolooABailRepository _aBailRepository = unitOfWork.GetHolooRepository<HolooABailRepository, HolooABail>();
    private readonly IHolooFBailRepository _fBailRepository = unitOfWork.GetHolooRepository<HolooFBailRepository, HolooFBail>();

    [HttpPost]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<IActionResult> Post(FactorViewModel? factor, CancellationToken cancellationToken)
    {
        try
        {
            if (factor == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });

            factor.HolooFBail.Fac_Type = "P";
            var bailCode = await _fBailRepository.Add(factor.HolooFBail, cancellationToken);
            if (bailCode == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Error,
                    Messages = new List<string> { "مشکل در ثبت فاکتور" }
                });
            foreach (var t in factor.HolooABails)
            {
                t.Fac_Code = bailCode;
                t.Fac_Type = "P";
            }

            _aBailRepository.Add(factor.HolooABails);
            await unitOfWork.SaveAsync(cancellationToken, true);
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