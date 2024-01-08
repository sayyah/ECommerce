using ECommerce.API.Commands;
using ECommerce.API.Queries;
using MediatR;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class BrandsController : ControllerBase
{
    private readonly IBrandRepository _brandRepository;
    private readonly ILogger<BrandsController> _logger;
    private readonly IMediator _mediator;

    public BrandsController(IBrandRepository brandRepository, ILogger<BrandsController> logger, IMediator mediator)
    {
        _brandRepository = brandRepository;
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    ///     Get All Brands.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllBrandsQuery();
        var brands = await _mediator.Send(query, CancellationToken.None);
        try
        {

            brands.Insert(0, new Brand
            {
                Name = "بدون برند"
            });
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = brands
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWithPagination([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity = await _brandRepository.Search(paginationParameters, cancellationToken);
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

            //var metadata = new
            //{
            //    entity.TotalCount,
            //    entity.PageSize,
            //    entity.CurrentPage,
            //    entity.TotalPages,
            //    entity.HasNext,
            //    entity.HasPrevious
            //};
            //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(new ApiResult
            {
                PaginationDetails = paginationDetails,
                Code = ResultCode.Success,
                ReturnData = entity
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpGet]
    public async Task<ActionResult<Brand>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var x = User.Identity.Name;
            var query = new GetByIDBrandQuery(id);
            var result = await _mediator.Send(query, CancellationToken.None);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Post(Brand brand, CancellationToken cancellationToken)
    {
        try
        {
            if (brand == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });

            var query = new PostBrandRequest(brand);
            var result = await _mediator.Send(query);

            if (result != null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "نام برند تکراری است" }
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = result
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
            { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> Put(Brand brand, CancellationToken cancellationToken)
    {
        try
        {
            var query = new PutBrandRequest(brand);
            var result = _mediator.Send(query);
            if ( result != null )
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "نام برند تکراری است" }
                });
            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
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
            var query = new DeleteBrandRequest(id);
            await _mediator.Send(query);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }
}