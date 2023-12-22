namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProductCommentsController(IUnitOfWork unitOfWork, ILogger<ProductCommentsController> logger)
    : ControllerBase
{
    private readonly IImageRepository _imageRepository = unitOfWork.GetRepository<IImageRepository, Image>();
    private readonly IProductCommentRepository _productCommentRepository = unitOfWork.GetRepository<IProductCommentRepository, ProductComment>();

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity = await _productCommentRepository.Search(paginationParameters, cancellationToken);
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
    public async Task<ActionResult<ProductComment>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = _productCommentRepository.GetByIdWithInclude("Answer,Product", id);
            if (result is not { Product: not null })
                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = result
                });

            result.Product.Images = await _imageRepository.GetByProductId(result.Product.Id, cancellationToken);
            result.Answer ??= new ProductComment();

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
    public async Task<IActionResult> GetAllAcceptedComments([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity =
                await _productCommentRepository.GetAllAcceptedComments(paginationParameters, cancellationToken);
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

    [HttpPost]
    public async Task<IActionResult> Post(ProductComment? productComment, CancellationToken cancellationToken)
    {
        try
        {
            if (productComment == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });

            productComment.IsAccepted = false;
            productComment.IsRead = false;
            productComment.IsAnswered = false;
            productComment.DateTime = DateTime.Now;

            _productCommentRepository.Add(productComment);
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
    public async Task<ActionResult<bool>> Put(ProductComment? productComment, CancellationToken cancellationToken)
    {
        try
        {
            ProductComment? commentAnswer;
            if (productComment == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });

            if (productComment is { AnswerId: not null })
            {
                commentAnswer =
                    await _productCommentRepository.GetByIdAsync(cancellationToken, productComment.AnswerId);
                if (commentAnswer != null && productComment.Answer != null) 
                {
                    commentAnswer.Text = productComment.Answer.Text;
                    commentAnswer.DateTime = DateTime.Now;
                    _productCommentRepository.Update(commentAnswer);
                    await unitOfWork.SaveAsync(cancellationToken);
                }
            }
            else
            {
                if (productComment.Answer?.Text != null)
                {
                    productComment.Answer.Name = "پاسخ ادمین";
                    productComment.Answer.IsAccepted = false;
                    productComment.Answer.IsRead = false;
                    productComment.Answer.IsAnswered = false;
                    productComment.Answer.DateTime = DateTime.Now;
                    commentAnswer = await _productCommentRepository.AddAsync(productComment.Answer, cancellationToken);
                    if (commentAnswer != null)
                    {
                        productComment.Answer = commentAnswer;
                        productComment.AnswerId = commentAnswer.Id;
                    }
                }
            }

            _productCommentRepository.Update(productComment);
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
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _productCommentRepository.DeleteById(id, cancellationToken);
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