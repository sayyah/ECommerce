namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class SendInformationController(ISendInformationRepository sendInformationRepository,
        ILogger<BrandsController> logger)
    : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<IActionResult> GetByUserId(int id, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await sendInformationRepository.Where(x => x.UserId == id, cancellationToken)
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
    public async Task<ActionResult<SendInformation>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sendInformationRepository.GetByIdAsync(cancellationToken, id);
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
    public async Task<IActionResult> Post(SendInformation sendInformation, CancellationToken cancellationToken)
    {
        try
        {
            if (sendInformation == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });
            sendInformation.Address = sendInformation.Address.Trim();

            var repetitive = await sendInformationRepository.Where(
                x => x.UserId == sendInformation.UserId && x.RecipientName.Equals(sendInformation.RecipientName) &&
                     x.Address.Equals(sendInformation.Address), cancellationToken);
            if (repetitive.Any())
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "آدرس تکراری است" }
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await sendInformationRepository.AddAsync(sendInformation, cancellationToken)
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
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> Put(
        SendInformation sendInformation,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var repetitive = await sendInformationRepository.Where(
                x =>
                    x.Id != sendInformation.Id
                    && x.UserId == sendInformation.UserId
                    && x.RecipientName.Equals(sendInformation.RecipientName)
                    && x.Mobile.Equals(sendInformation.Mobile)
                    && x.Address.Equals(sendInformation.Address)
                    && x.StateId == sendInformation.StateId
                    && x.CityId == sendInformation.CityId
                    && x.PostalCode!.Equals(sendInformation.PostalCode),
                cancellationToken
            );
            if (repetitive.Any())
                return Ok(
                    new ApiResult
                    {
                        Code = ResultCode.Repetitive,
                        Messages = new List<string> { "آدرس تکراری است" }
                    }
                );

            await sendInformationRepository.UpdateAsync(sendInformation, cancellationToken);
            return Ok(new ApiResult { Code = ResultCode.Success });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(
                new ApiResult
                {
                    Code = ResultCode.DatabaseError,
                    Messages = new List<string> { "اشکال در سمت سرور" }
                }
            );
        }
    }

    [HttpDelete]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await sendInformationRepository.DeleteAsync(id, cancellationToken);
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