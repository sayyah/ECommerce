using AutoMapper;
using ECommerce.API.DataTransferObject.BlogComments.Commands;
using ECommerce.API.DataTransferObject.BlogComments.Queries;
using ECommerce.API.DataTransferObject.Blogs.Commands;
using ECommerce.API.DataTransferObject.Blogs.Queris;
using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogComments.Commands;
using ECommerce.Application.Services.BlogComments.Queries;
using ECommerce.Application.Services.BlogComments.Result;
using ECommerce.Application.Services.Blogs.Commands;
using ECommerce.Application.Services.Blogs.Queries;
using ECommerce.Application.Services.Blogs.Results;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class BlogCommentsController(IMapper mapper)
    : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<ICollection<ReadBlogCommentDto>>> Get(
        [FromQuery] GetBlogCommentQueryDto getBlogCommentQueryDto,
        [FromServices] IQueryHandler<GetBlogCommentQuery, PagedList<BlogCommentResult>> queryHandler)
    {
        if (string.IsNullOrEmpty(getBlogCommentQueryDto.PaginationParameters.Search))
        {
            getBlogCommentQueryDto.PaginationParameters.Search = "";
        }

        GetBlogCommentQuery query = mapper.Map<GetBlogCommentQuery>(getBlogCommentQueryDto);
        var blogComments = await queryHandler.HandleAsync(query);
        PagedList<ReadBlogCommentDto> blogCommentDto = mapper.Map<PagedList<ReadBlogCommentDto>>(blogComments);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = blogCommentDto
        });
    }

    [HttpGet]
    public async Task<ActionResult<ReadBlogCommentDto>> GetById(
        [FromQuery] GetBlogCommentByIdQueryDto getBlogCommentByIdQueryDto,
        [FromServices] IQueryHandler<GetBlogCommentByIdQuery, BlogCommentResult> queryHandler)
    {
        GetBlogCommentByIdQuery query = mapper.Map<GetBlogCommentByIdQuery>(getBlogCommentByIdQueryDto);
        var blogComments = await queryHandler.HandleAsync(query);
        ReadBlogCommentDto blogCommentDto = mapper.Map<ReadBlogCommentDto>(blogComments);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = blogCommentDto
        });
    }

    [HttpGet]
    public async Task<ActionResult<ReadBlogCommentDto>> GetAllAcceptedComments(
        [FromQuery] GetBlogCommentAllAcceptedQueryDto getBlogCommentAllAcceptedQueryDto,
        [FromServices] IQueryHandler<GetBlogCommentAllAcceptedQuery, PagedList<BlogCommentResult>> queryHandler)
    {
        if (string.IsNullOrEmpty(getBlogCommentAllAcceptedQueryDto.PaginationParameters.Search))
        {
            getBlogCommentAllAcceptedQueryDto.PaginationParameters.Search = "";
        }

        GetBlogCommentAllAcceptedQuery query = mapper.Map<GetBlogCommentAllAcceptedQuery>(getBlogCommentAllAcceptedQueryDto);
        var blogComments = await queryHandler.HandleAsync(query);
        PagedList<ReadBlogCommentDto> blogCommentDto = mapper.Map<PagedList<ReadBlogCommentDto>>(blogComments);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = blogCommentDto
        });
    }

    [HttpPost]
    public async Task<ActionResult<ReadBlogCommentDto>> Post(
        [FromQuery] CreateBlogCommentDto createBlogCommentDto,
        [FromServices] ICommandHandler<CreateBlogCommentCommand, bool> commandHandler,
        CancellationToken cancellationToken)
    {
        CreateBlogCommentCommand command = mapper.Map<CreateBlogCommentCommand>(createBlogCommentDto);
        bool isSuccess = await commandHandler.HandleAsync(command, cancellationToken);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = isSuccess
        });
    }

    [HttpPut]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<ReadBlogCommentDto>> Put(
        [FromQuery] EditBlogCommentDto editBlogCommentDto,
        [FromServices] ICommandHandler<EditBlogCommentCommand, bool> commandHandler,
        CancellationToken cancellationToken)
    {
        EditBlogCommentCommand command = mapper.Map<EditBlogCommentCommand>(editBlogCommentDto);
        bool isSuccess = await commandHandler.HandleAsync(command, cancellationToken);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = isSuccess
        });
    }

    [HttpDelete]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult> Delete(
        [FromQuery] DeleteBlogCommentDto deleteBlogCommentDto,
        [FromServices] ICommandHandler<DeleteBlogCommentCommand, bool> commandHandler,
        CancellationToken cancellationToken)
    {
        DeleteBlogCommentCommand command = mapper.Map<DeleteBlogCommentCommand>(deleteBlogCommentDto);
        bool isSuccess = await commandHandler.HandleAsync(command, cancellationToken);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = isSuccess
        });
    }
}
