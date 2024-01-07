namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ImagesController(IHostEnvironment environment, IImageRepository imageRepository,
        ILogger<ImagesController> logger)
    : ControllerBase
{
    private readonly IHostEnvironment _environment = environment;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity = await imageRepository.Search(paginationParameters, cancellationToken);
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
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetByProductId(int productId, CancellationToken cancellationToken)
    {
        var result = await imageRepository.GetByProductId(productId, cancellationToken);
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

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Post(Image image, CancellationToken cancellationToken)
    {
        try
        {
            var addedImage = await imageRepository.AddAsync(image, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = addedImage.Id
            });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Put(Image image, CancellationToken cancellationToken)
    {
        try
        {
            var addedImage = await imageRepository.AddAsync(image, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = addedImage.Id
            });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var image = imageRepository.GetById(id);
        await imageRepository.DeleteByName(image.Name, cancellationToken);

        return Ok(new ApiResult
        {
            Code = ResultCode.Success
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetByBlogId(int blogId, CancellationToken cancellationToken)
    {
        var result = await imageRepository.GetByBlogId(blogId, cancellationToken);
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
}