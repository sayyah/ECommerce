﻿using AutoMapper;
using ECommerce.API.DataTransferObject.Blogs;
using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.Blogs.Queries;
using ECommerce.Application.Services.Blogs.Results;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class BlogsController(IMapper mapper, ILogger<BlogsController> logger) : ControllerBase
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
        if (string.IsNullOrEmpty(getBlogByTagTextQueryDto.PaginationParameters.TagText))
            getBlogByTagTextQueryDto.PaginationParameters.TagText = "";
        GetBlogByTagTextQuery query = mapper.Map<GetBlogByTagTextQuery>(getBlogByTagTextQueryDto);
        var blogs = await queryHandler.HandleAsync(query);
        PagedList<ReadBlogDto> blogDto = mapper.Map<PagedList<ReadBlogDto>>(blogs);
        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            ReturnData = blogDto
        });
    }



    //[HttpPost]
    //[Authorize(Roles = "Admin,SuperAdmin")]
    //public async Task<IActionResult> Post(BlogViewModel? blogViewModel, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        if (blogViewModel == null)
    //            return Ok(new ApiResult
    //            {
    //                Code = ResultCode.BadRequest
    //            });
    //        blogViewModel.Title = blogViewModel.Title.Trim();

    //        var repetitiveTitle = await _blogRepository.GetByTitle(blogViewModel.Title, cancellationToken);
    //        if (repetitiveTitle != null)
    //            return Ok(new ApiResult
    //            {
    //                Code = ResultCode.Repetitive,
    //                Messages = new List<string> { "عنوان مقاله تکراری است" }
    //            });
    //        var repetitiveUrl = await _blogRepository.GetByUrl(blogViewModel.Url, cancellationToken);
    //        if (repetitiveUrl != null)
    //            return Ok(new ApiResult
    //            {
    //                Code = ResultCode.Repetitive,
    //                Messages = new List<string> { "آدرس مقاله تکراری است" }
    //            });

    //        var newBlog = await _blogRepository.AddWithRelations(blogViewModel, cancellationToken);
    //        await unitOfWork.SaveAsync(cancellationToken);

    //        return Ok(new ApiResult
    //        {
    //            Code = ResultCode.Success,
    //            ReturnData = newBlog
    //        });
    //    }
    //    catch (Exception e)
    //    {
    //        logger.LogCritical(e, e.Message);
    //        return Ok(new ApiResult { Code = ResultCode.DatabaseError });
    //    }
    //}

    //[HttpPut]
    //[Authorize(Roles = "Admin,SuperAdmin")]
    //public async Task<ActionResult<int>> Put(BlogViewModel? blogViewModel, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        if (blogViewModel == null) return BadRequest();


    //        var repetitiveTitle = await _blogRepository.GetByTitle(blogViewModel.Title, cancellationToken);
    //        if (repetitiveTitle != null && repetitiveTitle.Id != blogViewModel.Id)
    //            return Ok(new ApiResult
    //            {
    //                Code = ResultCode.Repetitive,
    //                Messages = new List<string> { "عنوان مقاله تکراری است" }
    //            });
    //        if (repetitiveTitle != null) _blogRepository.Detach(repetitiveTitle);
    //        var repetitiveUrl = await _blogRepository.GetByUrl(blogViewModel.Url, cancellationToken);
    //        if (repetitiveUrl != null && repetitiveUrl.Id != blogViewModel.Id)
    //            return Ok(new ApiResult
    //            {
    //                Code = ResultCode.Repetitive,
    //                Messages = new List<string> { "آدرس مقاله تکراری است" }
    //            });
    //        if (repetitiveUrl != null) _blogRepository.Detach(repetitiveUrl);
    //        await _blogRepository.EditWithRelations(blogViewModel, cancellationToken);
    //        await unitOfWork.SaveAsync(cancellationToken);

    //        return Ok(new ApiResult
    //        {
    //            Code = ResultCode.Success
    //        });
    //    }
    //    catch (Exception e)
    //    {
    //        logger.LogCritical(e, e.Message);
    //        return Ok(new ApiResult { Code = ResultCode.DatabaseError });
    //    }
    //}

    //[HttpDelete]
    //[Authorize(Roles = "SuperAdmin")]
    //public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        await _blogRepository.DeleteById(id, cancellationToken);
    //        await unitOfWork.SaveAsync(cancellationToken);

    //        return Ok(new ApiResult
    //        {
    //            Code = ResultCode.Success
    //        });
    //    }
    //    catch (Exception e)
    //    {
    //        logger.LogCritical(e, e.Message);
    //        return Ok(new ApiResult { Code = ResultCode.DatabaseError });
    //    }
    //}

    //[HttpGet]
    //public async Task<IActionResult> GetByCategory(int id, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        return Ok(new ApiResult
    //        {
    //            Code = ResultCode.Success,
    //            ReturnData = await _blogRepository.GetWithInclude(id, cancellationToken)
    //        });
    //    }
    //    catch (Exception e)
    //    {
    //        logger.LogCritical(e, e.Message);
    //        return Ok(new ApiResult { Code = ResultCode.DatabaseError });
    //    }
    //}

    //[HttpGet]
    //public async Task<ActionResult<BlogViewModel>> GetByUrl(string blogUrl, bool isColleague,
    //    CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        var result = await _blogRepository.GetBlogByUrlWithInclude(blogUrl).FirstOrDefaultAsync(cancellationToken);
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
    //        return Ok(new ApiResult { Code = ResultCode.DatabaseError });
    //    }
    //}
}