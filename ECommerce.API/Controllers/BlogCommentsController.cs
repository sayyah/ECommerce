// Ignore Spelling: Blog

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class BlogCommentsController(IUnitOfWork unitOfWork, ILogger<BlogCommentsController> logger)
    : ControllerBase
{
    private readonly IBlogCommentRepository _blogCommentRepository = unitOfWork.GetRepository<IBlogCommentRepository,BlogComment>();
    private readonly IImageRepository _imageRepository = unitOfWork.GetRepository < IImageRepository,Image>();

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity = await _blogCommentRepository.Search(paginationParameters, cancellationToken);
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
            var result = _blogCommentRepository.GetByIdWithInclude("Answer,Blog", id);
            if (result?.Blog == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.NotFound
                });

            result.Blog.Image = await _imageRepository.GetByBlogId(result.Blog.Id, cancellationToken);
            result.Answer ??= new BlogComment();

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
    public async Task<IActionResult> Post(BlogComment? blogComment, CancellationToken cancellationToken)
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
            _blogCommentRepository.Add(blogComment);
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
    public async Task<ActionResult<bool>> Put(BlogComment blogComment, CancellationToken cancellationToken)
    {
        try
        {
            BlogComment? commentAnswer;
            if (blogComment.AnswerId != null)
            {
                commentAnswer = await _blogCommentRepository.GetByIdAsync(cancellationToken, blogComment.AnswerId);
                if (commentAnswer != null)
                {
                    commentAnswer.DateTime = DateTime.Now;
                    _blogCommentRepository.Update(commentAnswer);
                }
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
                    commentAnswer = await _blogCommentRepository.AddAsync(blogComment.Answer, cancellationToken);
                   
                    if (commentAnswer != new BlogComment())
                    {
                        blogComment.Answer = commentAnswer;
                        blogComment.AnswerId = commentAnswer.Id;
                    }
                }
            }

            _blogCommentRepository.Update(blogComment);
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
            await _blogCommentRepository.DeleteById(id, cancellationToken);
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
    public async Task<IActionResult> GetAllAcceptedComments([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity = await _blogCommentRepository.GetAllAcceptedComments(paginationParameters, cancellationToken);
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
