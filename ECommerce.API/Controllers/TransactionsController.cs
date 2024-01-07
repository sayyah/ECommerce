namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TransactionsController(ILogger<PurchaseOrdersController> logger,
        ITransactionRepository transactionRepository)
    : Controller
{
    [HttpGet]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<PurchaseOrder>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await transactionRepository.GetByIdAsync(cancellationToken, id);
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
    public async Task<IActionResult> Get([FromQuery] TransactionFiltreViewModel transactionFiltreViewModel,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(transactionFiltreViewModel.PaginationParameters.Search))
                transactionFiltreViewModel.PaginationParameters.Search = "";
            var entity = await transactionRepository.Search(transactionFiltreViewModel, cancellationToken);
            var paginationDetails = new PaginationDetails
            {
                TotalCount = entity.TotalCount,
                PageSize = entity.PageSize,
                CurrentPage = entity.CurrentPage,
                TotalPages = entity.TotalPages,
                HasNext = entity.HasNext,
                HasPrevious = entity.HasPrevious,
                Search = transactionFiltreViewModel.PaginationParameters.Search
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
                ReturnData = await transactionRepository.GetAll(cancellationToken)
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }
}