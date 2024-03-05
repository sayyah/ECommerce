using AutoMapper;
using ECommerce.API.DataTransferObject.BlogCategories;
using ECommerce.API.DataTransferObject.BlogCategories.Commands;
using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogCategories.Commands;
using ECommerce.Application.Services.BlogCategories.Queries;
using ECommerce.Application.Services.BlogCategories.Results;
using ECommerce.Application.Services.Blogs.Commands;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class BlogCategoriesController(IMapper mapper, ILogger<BlogCategoriesController> logger) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<ICollection<ReadBlogCategoryDto>>> GetAllWithPagination(
        [FromQuery] GetBlogCategoriesQueryDto getBlogCategoriesQueryDto,
        [FromServices] IQueryHandler<GetBlogCategoriesQuery, PagedList<BlogCategoryResult>> queryHandler)
    {

        try
        {
            if (string.IsNullOrEmpty(getBlogCategoriesQueryDto.PaginationParameters.Search))
            {
                getBlogCategoriesQueryDto.PaginationParameters.Search = "";
            }
            GetBlogCategoriesQuery query = mapper.Map<GetBlogCategoriesQuery>(getBlogCategoriesQueryDto);
            var blogCategories = await queryHandler.HandleAsync(query);
            PagedList<ReadBlogCategoryDto> blogCategoryDto = mapper.Map<PagedList<ReadBlogCategoryDto>>(blogCategories);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = blogCategoryDto
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
          
    }

    [HttpGet]
    public async Task<ActionResult<ReadBlogCategoryDto>> GetById(
        [FromQuery] GetBlogCategoryByIdQueryDto getBlogCategoryByIdQueryDto,
        [FromServices] IQueryHandler<GetBlogCategoryByIdQuery, BlogCategoryResult> queryHandler)
    {
        GetBlogCategoryByIdQuery query = mapper.Map<GetBlogCategoryByIdQuery>(getBlogCategoryByIdQueryDto);
        var blogCategories = await queryHandler.HandleAsync(query);
        ReadBlogCategoryDto blogCategoryDto = mapper.Map<ReadBlogCategoryDto>(blogCategories);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = blogCategoryDto
        });
    }


    [HttpGet]
    public async Task<ActionResult<List<ReadBlogCategoryParentDto>>> GetParents(
        [FromQuery] GetBlogParentCategoryByIdQueryDto getBlogParentCategoryByIdQueryDto,
        [FromServices] IQueryHandler<GetBlogParentCategoryByIdQuery, List<BLogCategoryParentResult>> queryHandler)
    {
        GetBlogParentCategoryByIdQuery query = mapper.Map<GetBlogParentCategoryByIdQuery>(getBlogParentCategoryByIdQueryDto);
        var blogCategories = await queryHandler.HandleAsync(query);
        List<ReadBlogCategoryParentDto> blogCategoryParentDto = mapper.Map<List<ReadBlogCategoryParentDto>>(blogCategories);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = blogCategoryParentDto
        });
    }


    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Post(
        [FromQuery] CreateBlogCategoryDto createBlogCategoryDto,
        [FromServices] ICommandHandler<CreateBlogCategoryCommand, bool> commandHandler,
        CancellationToken cancellationToken)
    {
        CreateBlogCategoryCommand command = mapper.Map<CreateBlogCategoryCommand>(createBlogCategoryDto);
        bool isSuccess = await commandHandler.HandleAsync(command, cancellationToken);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = isSuccess
        });
    }

    [HttpPut]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> Put(
        [FromQuery] EditBlogCategoryDto editBlogCategoryDto,
        [FromServices] ICommandHandler<EditBlogCategoryCommand, bool> commandHandler,
        CancellationToken cancellationToken)
    {
        EditBlogCategoryCommand command = mapper.Map<EditBlogCategoryCommand>(editBlogCategoryDto);
        bool isSuccess = await commandHandler.HandleAsync(command, cancellationToken);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = isSuccess
        });
    }

    [HttpDelete]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(
        [FromQuery] DeleteBlogCategoryDto deleteBlogCategory,
        [FromServices] ICommandHandler<DeleteBlogCategoryCommand, bool> commandHandler,
        CancellationToken cancellationToken)
    {
        DeleteBlogCategoryCommand command = mapper.Map<DeleteBlogCategoryCommand>(deleteBlogCategory);
        bool isSuccess = await commandHandler.HandleAsync(command, cancellationToken);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = isSuccess
        });
    }
}
