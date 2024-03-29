﻿namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class KeywordsController(IUnitOfWork unitOfWork, ILogger<KeywordsController> logger) : ControllerBase
{
    private readonly IKeywordRepository _keywordRepository = unitOfWork.GetRepository<KeywordRepository, Keyword>();

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity = await _keywordRepository.Search(paginationParameters, cancellationToken);
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
                ReturnData = await _keywordRepository.GetAllAsync(cancellationToken)
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetKeywordsByProductId(int id, CancellationToken cancellationToken)
    {
        try
        {
            var keywordList = await _keywordRepository.GetByProductId(id, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = keywordList
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    public async Task<ActionResult<Keyword>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _keywordRepository.GetByIdAsync(cancellationToken, id);
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
    public async Task<IActionResult> Post(Keyword? keywords, CancellationToken cancellationToken)
    {
        try
        {
            if (keywords == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });
            keywords.KeywordText = keywords.KeywordText.Trim();

            var repetitiveCategory = await _keywordRepository.GetByKeywordText(keywords.KeywordText, cancellationToken);
            if (repetitiveCategory != null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "کلمه کلیدی تکراری است" }
                });

            _keywordRepository.Add(keywords);
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
    public async Task<ActionResult<bool>> Put(Keyword keyword, CancellationToken cancellationToken)
    {
        try
        {
            var repetitive = await _keywordRepository.GetByKeywordText(keyword.KeywordText, cancellationToken);
            if (repetitive != null && repetitive.Id != keyword.Id)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "کلمه کلیدی تکراری است" }
                });
            if (repetitive != null) _keywordRepository.Detach(repetitive);
            _keywordRepository.Update(keyword);
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
            await _keywordRepository.DeleteById(id, cancellationToken);
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
}