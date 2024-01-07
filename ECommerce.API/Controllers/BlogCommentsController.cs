namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class BlogCommentsController(IBlogCommentRepository blogCommentRepository, ILogger<BlogCommentsController> logger
        , IImageRepository imageRepository)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity = await blogCommentRepository.Search(paginationParameters, cancellationToken);
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
    public async Task<ActionResult<BlogComment>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = blogCommentRepository.GetByIdWithInclude("Answer,Blog", id);
            result.Blog.Image = await imageRepository.GetByBlogId(result.Blog.Id, cancellationToken);
            if (result.Answer == null) result.Answer = new BlogComment();
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
    public async Task<IActionResult> Post(BlogComment blogComment, CancellationToken cancellationToken)
    {
        try
        {
            if (blogComment == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });

            blogComment.IsAccepted = false;
            blogComment.IsRead = false;
            blogComment.IsAnswered = false;
            blogComment.DateTime = DateTime.Now;

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await blogCommentRepository.AddAsync(blogComment, cancellationToken)
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
    public async Task<ActionResult<bool>> Put(BlogComment blogComment, CancellationToken cancellationToken)
    {
        try
        {
            BlogComment? _commentAnswer;
            if (blogComment.AnswerId != null)
            {
                _commentAnswer = await blogCommentRepository.GetByIdAsync(cancellationToken, blogComment.AnswerId);
                _commentAnswer.DateTime = DateTime.Now;
                await blogCommentRepository.UpdateAsync(_commentAnswer, cancellationToken);
            }
            else
            {
                if (blogComment.Answer?.Text != null)
                {
                    blogComment.Answer.Name = "پاسخ ادمین";
                    blogComment.Answer.IsAccepted = false;
                    blogComment.Answer.IsRead = false;
                    blogComment.Answer.IsAnswered = false;
                    blogComment.Answer.DateTime = DateTime.Now;
                    _commentAnswer = await blogCommentRepository.AddAsync(blogComment.Answer, cancellationToken);
                    if (_commentAnswer != null)
                    {
                        blogComment.Answer = _commentAnswer;
                        blogComment.AnswerId = _commentAnswer.Id;
                    }
                }
            }

            await blogCommentRepository.UpdateAsync(blogComment, cancellationToken);
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
            await blogCommentRepository.DeleteAsync(id, cancellationToken);
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
    public async Task<IActionResult> GetAllAccesptedComments([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity = await blogCommentRepository.GetAllAccesptedComments(paginationParameters, cancellationToken);
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
}
