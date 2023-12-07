namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TagsController(IUnitOfWork unitOfWork, ILogger<TagsController> logger) : ControllerBase
{
    private readonly ITagRepository _tagRepository = unitOfWork.GetRepository<ITagRepository, Tag>();

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity = await _tagRepository.Search(paginationParameters, cancellationToken);
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
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await _tagRepository.GetAllAsync(cancellationToken)
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetByProductId(int id, CancellationToken cancellationToken)
    {
        try
        {
            var tagList = await _tagRepository.GetByProductId(id, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = tagList
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    public async Task<ActionResult<Tag>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _tagRepository.GetByIdAsync(cancellationToken, id);
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

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Post(Tag? tag, CancellationToken cancellationToken)
    {
        try
        {
            if (tag == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });
            tag.TagText = tag.TagText.Trim();

            var repetitiveTag = await _tagRepository.GetByTagText(tag.TagText, cancellationToken);
            if (repetitiveTag != null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "تگ تکراری است" }
                });

            _tagRepository.Add(tag);
            await unitOfWork.SaveAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> Put(Tag tag, CancellationToken cancellationToken)
    {
        try
        {
            var repetitive = await _tagRepository.GetByTagText(tag.TagText, cancellationToken);
            if (repetitive != null && repetitive.Id != tag.Id)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "تگ تکراری است" }
                });
            if (repetitive != null) _tagRepository.Detach(repetitive);
             _tagRepository.Update(tag);
            await unitOfWork.SaveAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpDelete]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _tagRepository.DeleteById(id, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProductTags(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await _tagRepository.GetAllProductTags(cancellationToken)
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBlogTags(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await _tagRepository.GetAllBlogTags(cancellationToken)
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }
}