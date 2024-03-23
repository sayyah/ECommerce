using AutoMapper;
using ECommerce.API.DataTransferObject.Blogs.Commands;
using ECommerce.API.DataTransferObject.Blogs.Queries;
using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.Blogs.Commands;
using ECommerce.Application.Services.Blogs.Queries;
using ECommerce.Application.Services.Blogs.Results;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class BlogsController(IMapper mapper) : ControllerBase
{


    [HttpGet]
    public async Task<ActionResult<ICollection<ReadBlogDto>>> Get(
        [FromQuery] GetBlogsQueryDto getBlogsQueryDto,
        [FromServices] IQueryHandler<GetBlogsQuery, PagedList<BlogResult>> queryHandler)
    {
        if (string.IsNullOrEmpty(getBlogsQueryDto.PaginationParameters.Search))
        {
            getBlogsQueryDto.PaginationParameters.Search = "";
        }

        GetBlogsQuery query = mapper.Map<GetBlogsQuery>(getBlogsQueryDto);
        var blogs = await queryHandler.HandleAsync(query);
        PagedList<ReadBlogDto> blogDto = mapper.Map<PagedList<ReadBlogDto>>(blogs);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = blogDto
        });
    }

    [HttpGet]
    public async Task<ActionResult<ReadBlogDto>> GetById(
        [FromQuery] GetBlogByIdQueryDto getBlogByIdQueryDto,
        [FromServices] IQueryHandler<GetBlogByIdQuery, BlogResult> queryHandler)
    {
        GetBlogByIdQuery query = mapper.Map<GetBlogByIdQuery>(getBlogByIdQueryDto);
        var blogs = await queryHandler.HandleAsync(query);
        ReadBlogDto blogDto = mapper.Map<ReadBlogDto>(blogs);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = blogDto
        });
    }

    [HttpGet]
    public async Task<ActionResult<ReadBlogDto>> GetByTagText(
        [FromQuery] GetBlogByTagTextQueryDto getBlogByTagTextQueryDto,
        [FromServices] IQueryHandler<GetBlogByTagTextQuery, PagedList<BlogResult>> queryHandler)
    {
        if (string.IsNullOrEmpty(getBlogByTagTextQueryDto.TagText))
            getBlogByTagTextQueryDto.TagText = "";
        GetBlogByTagTextQuery query = mapper.Map<GetBlogByTagTextQuery>(getBlogByTagTextQueryDto);
        var blogs = await queryHandler.HandleAsync(query);
        PagedList<ReadBlogDto> blogDto = mapper.Map<PagedList<ReadBlogDto>>(blogs);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = blogDto
        });
    }

    [HttpGet]
    public async Task<ActionResult<ReadBlogDto>> GetByCategory(
        [FromQuery] GetBlogByCategoryQueryDto getBlogByCategoryQueryDto,
        [FromServices] IQueryHandler<GetBlogByCategoryQuery, PagedList<BlogResult>> queryHandler)
    {
        GetBlogByCategoryQuery query = mapper.Map<GetBlogByCategoryQuery>(getBlogByCategoryQueryDto);
        var blogs = await queryHandler.HandleAsync(query);
        PagedList<ReadBlogDto> blogDto = mapper.Map<PagedList<ReadBlogDto>>(blogs);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = blogDto
        });
    }

    [HttpGet]
    public async Task<ActionResult<ReadBlogDto>> GetByUrl(
        [FromQuery] GetBlogByUrlQueryDto getBlogByUrlQueryDto,
        [FromServices] IQueryHandler<GetBlogByUrlQuery, BlogResult> queryHandler)
    {
        GetBlogByUrlQuery query = mapper.Map<GetBlogByUrlQuery>(getBlogByUrlQueryDto);
        var blogs = await queryHandler.HandleAsync(query);
        ReadBlogDto blogDto = mapper.Map<ReadBlogDto>(blogs);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = blogDto
        });
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<ReadBlogDto>> Post(
        [FromQuery] CreateBlogDto createBlogDto,
        [FromServices] ICommandHandler<CreateBlogCommand, bool> commandHandler,
        CancellationToken cancellationToken)
    {
        CreateBlogCommand command = mapper.Map<CreateBlogCommand>(createBlogDto);
        bool isSuccess = await commandHandler.HandleAsync(command, cancellationToken);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = isSuccess
        });
    }

    [HttpPut]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<ReadBlogDto>> Put(
        [FromQuery] EditBlogDto editeBlogDto,
        [FromServices] ICommandHandler<EditBlogCommand, bool> commandHandler,
        CancellationToken cancellationToken)
    {
        EditBlogCommand command = mapper.Map<EditBlogCommand>(editeBlogDto);
        bool isSuccess = await commandHandler.HandleAsync(command,cancellationToken);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = isSuccess
        });
    }

    [HttpDelete]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult> Delete(
        [FromQuery] DeleteBlogDto deleteBlogDto,
        [FromServices] ICommandHandler<DeleteBlogCommand, bool> commandHandler,
        CancellationToken cancellationToken)
    {
        DeleteBlogCommand command = mapper.Map<DeleteBlogCommand>(deleteBlogDto);
        bool isSuccess = await commandHandler.HandleAsync(command, cancellationToken);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = isSuccess
        });
    }
}