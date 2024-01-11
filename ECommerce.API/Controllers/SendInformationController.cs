namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class SendInformationController(IUnitOfWork unitOfWork,
        ILogger<BrandsController> logger)
    : ControllerBase
{
    private readonly ISendInformationRepository _sendInformationRepository = unitOfWork.GetRepository<SendInformationRepository, SendInformation>();

    [HttpGet]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<IActionResult> GetByUserId(int id, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await _sendInformationRepository.Where(x => x.UserId == id, cancellationToken)
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
            var result = await _sendInformationRepository.GetByIdAsync(cancellationToken, id);
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
    public async Task<IActionResult> Post(SendInformation? sendInformation, CancellationToken cancellationToken)
    {
        try
        {
            if (sendInformation == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });
            sendInformation.Address = sendInformation.Address.Trim();

            var repetitive = await _sendInformationRepository.Where(
                x => x.UserId == sendInformation.UserId && x.RecipientName.Equals(sendInformation.RecipientName) &&
                     x.Address.Equals(sendInformation.Address), cancellationToken);
            if (repetitive != null && repetitive.Any())
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "آدرس تکراری است" }
                });
            _sendInformationRepository.Add(sendInformation);
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

    [HttpPut]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> Put(
        SendInformation sendInformation,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var repetitive = await _sendInformationRepository.Where(
                x => x.UserId == sendInformation.UserId && x.RecipientName.Equals(sendInformation.RecipientName) &&
                     x.Address.Equals(sendInformation.Address), cancellationToken);
            if (repetitive != null)
            {
                var sendInformationList = repetitive.ToList();
                if (sendInformationList.FirstOrDefault() != null && sendInformationList.FirstOrDefault()!.Id != sendInformation.Id)
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.Repetitive,
                        Messages = new List<string> { "آدرس تکراری است" }
                    });
            }

            _sendInformationRepository.Update(sendInformation);
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
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _sendInformationRepository.DeleteById(id, cancellationToken);
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