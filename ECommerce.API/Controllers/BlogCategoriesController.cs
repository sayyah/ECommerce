using AutoMapper;
using ECommerce.API.DataTransferObject.BlogCategories;
using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogCategories.Queries;
using ECommerce.Application.Services.BlogCategories.Results;

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

    //[HttpGet]
    //public async Task<ActionResult<BlogCategory>> GetById(int id, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        var result = await _blogCategoryRepository.GetByIdAsync(cancellationToken, id);
    //        if (result == null)
    //            return Ok(new ApiResult
    //            {
    //                Code = ResultCode.NotFound
    //            });

    //        return Ok(new ApiResult
    //        {
    //            Code = ResultCode.Success,
    //            ReturnData = result
    //        });
    //    }
    //    catch (Exception e)
    //    {
    //        logger.LogCritical(e, e.Message);
    //        return Ok(new ApiResult
    //            { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
    //    }
    //}


    //[HttpGet]
    //public async Task<IActionResult> GetParents(int blogId, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        var result = await _blogCategoryRepository.Parents(blogId, cancellationToken);
    //        if (result == null)
    //            return Ok(new ApiResult
    //            {
    //                Code = ResultCode.NotFound
    //            });

    //        return Ok(new ApiResult
    //        {
    //            Code = ResultCode.Success,
    //            ReturnData = result
    //        });
    //    }
    //    catch (Exception e)
    //    {
    //        logger.LogCritical(e, e.Message);
    //        return Ok(new ApiResult
    //            { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
    //    }
    //}


    //[HttpPost]
    //[Authorize(Roles = "Admin,SuperAdmin")]
    //public async Task<IActionResult> Post(BlogCategory? blogCategory, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        if (blogCategory == null)
    //            return Ok(new ApiResult
    //            {
    //                Code = ResultCode.BadRequest
    //            });
    //        blogCategory.Name = blogCategory.Name.Trim();

    //        var repetitiveCategory =
    //            await _blogCategoryRepository.GetByName(blogCategory.Name, blogCategory.Parent?.Id, cancellationToken);
    //        if (repetitiveCategory != null)
    //            return Ok(new ApiResult
    //            {
    //                Code = ResultCode.Repetitive,
    //                Messages = new List<string> { "نام دسته تکراری است" }
    //            });
    //        _blogCategoryRepository.Add(blogCategory);
    //        await unitOfWork.SaveAsync(cancellationToken);

    //        return Ok(new ApiResult
    //        {
    //            Code = ResultCode.Success,
    //        });
    //    }
    //    catch (Exception e)
    //    {
    //        logger.LogCritical(e, e.Message);
    //        return Ok(new ApiResult
    //            { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
    //    }
    //}

    //[HttpPut]
    //[Authorize(Roles = "Admin,SuperAdmin")]
    //public async Task<ActionResult<bool>> Put(BlogCategory blogCategory, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        _blogCategoryRepository.Update(blogCategory);
    //        await unitOfWork.SaveAsync(cancellationToken);
    //        return Ok(new ApiResult
    //        {
    //            Code = ResultCode.Success
    //        });
    //    }
    //    catch (Exception e)
    //    {
    //        logger.LogCritical(e, e.Message);
    //        return Ok(new ApiResult
    //            { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
    //    }
    //}

    //[HttpDelete]
    //[Authorize(Roles = "SuperAdmin")]
    //public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        await _blogCategoryRepository.DeleteById(id, cancellationToken);
    //        await unitOfWork.SaveAsync(cancellationToken);
    //        return Ok(new ApiResult
    //        {
    //            Code = ResultCode.Success
    //        });
    //    }
    //    catch (Exception e)
    //    {
    //        logger.LogCritical(e, e.Message);
    //        return Ok(new ApiResult
    //            { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
    //    }
    //}
}