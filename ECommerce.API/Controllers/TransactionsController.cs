namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TransactionsController(IUnitOfWork unitOfWork,
        ILogger<PurchaseOrdersController> logger)
    : Controller
{
    private readonly ITransactionRepository _transactionRepository = unitOfWork.GetRepository<TransactionRepository, Transaction>();

    [HttpGet]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<PurchaseOrder>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _transactionRepository.GetByIdAsync(cancellationToken, id);
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
    public IActionResult Get([FromQuery] transactionFilterViewModel? transactionFilterViewModel)
    {
        try
        {
            if (transactionFilterViewModel is { PaginationParameters: not null } &&
                string.IsNullOrEmpty(transactionFilterViewModel.PaginationParameters.Search))
            {
                transactionFilterViewModel.PaginationParameters.Search = "";
                var entity =  _transactionRepository.Search(transactionFilterViewModel);
                var paginationDetails = new PaginationDetails
                {
                    TotalCount = entity.TotalCount,
                    PageSize = entity.PageSize,
                    CurrentPage = entity.CurrentPage,
                    TotalPages = entity.TotalPages,
                    HasNext = entity.HasNext,
                    HasPrevious = entity.HasPrevious,
                    Search = transactionFilterViewModel.PaginationParameters.Search
                };

                return Ok(new ApiResult
                {
                    PaginationDetails = paginationDetails,
                    Code = ResultCode.Success,
                    ReturnData = entity
                });
            }
            return Ok(new ApiResult
            {
                Code = ResultCode.NotFound
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
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await _transactionRepository.GetAllAsync(cancellationToken)
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }
}