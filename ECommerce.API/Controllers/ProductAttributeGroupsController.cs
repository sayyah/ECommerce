namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProductAttributeGroupsController(IProductAttributeGroupRepository productAttributeGroupRepository,
        ILogger<ProductAttributeGroupsController> logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity = await productAttributeGroupRepository.Search(paginationParameters, cancellationToken);
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
                ReturnData = await productAttributeGroupRepository.GetAll(cancellationToken)
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    public async Task<ActionResult<ProductAttributeGroup>> GetByProductId(int productId,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await productAttributeGroupRepository.GetAllAttributeWithProductId(productId, cancellationToken);
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

    [HttpGet]
    public async Task<ActionResult<ProductAttributeGroup>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await productAttributeGroupRepository.GetByIdAsync(cancellationToken, id);
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
    public async Task<IActionResult> Post(ProductAttributeGroup productAttributeGroup,
        CancellationToken cancellationToken)
    {
        try
        {
            if (productAttributeGroup == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });
            productAttributeGroup.Name = productAttributeGroup.Name.Trim();

            var repetitiveName =
                await productAttributeGroupRepository.GetByName(productAttributeGroup.Name, cancellationToken);
            if (repetitiveName != null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "نام خصوصیت تکراری است" }
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await productAttributeGroupRepository.AddAsync(productAttributeGroup, cancellationToken)
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
    public async Task<IActionResult> AddWithAttributeValue(List<ProductAttributeGroup> productAttributeGroups,
        [FromQuery] int ProductId, CancellationToken cancellationToken)
    {
        try
        {
            if (productAttributeGroups == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData =
                    await productAttributeGroupRepository.AddWithAttributeValue(productAttributeGroups, ProductId,
                        cancellationToken)
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
    public async Task<ActionResult<bool>> Put(ProductAttributeGroup productAttributeGroup,
        CancellationToken cancellationToken)
    {
        try
        {
            var repetitive =
                await productAttributeGroupRepository.GetByName(productAttributeGroup.Name, cancellationToken);
            if (repetitive != null && repetitive.Id != productAttributeGroup.Id)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "نام خصوصیت تکراری است" }
                });
            if (repetitive != null) productAttributeGroupRepository.Detach(repetitive);
            await productAttributeGroupRepository.UpdateAsync(productAttributeGroup, cancellationToken);
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
            await productAttributeGroupRepository.DeleteAsync(id, cancellationToken);
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