namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UnitsController(IUnitRepository unitRepository, IHolooUnitRepository holooUnitRepository,
        ILogger<UnitsController> logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity = await unitRepository.Search(paginationParameters, cancellationToken);
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
    public async Task<IActionResult> GetHolooUnits(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await holooUnitRepository.GetAll(cancellationToken)
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    public async Task<ActionResult<Unit>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitRepository.GetByIdAsync(cancellationToken, id);
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
    public async Task<IActionResult> Post(Unit unit, CancellationToken cancellationToken)
    {
        try
        {
            if (unit == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });
            unit.Name = unit.Name.Trim();

            var repetitiveUnit = await unitRepository.GetByName(unit.Name, cancellationToken);
            if (repetitiveUnit != null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "نام واحد تکراری است" }
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await unitRepository.AddAsync(unit, cancellationToken)
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
    public async Task<ActionResult<bool>> Put(Unit unit, CancellationToken cancellationToken)
    {
        try
        {
            if (unit.Id == 1)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest,
                    Messages = new List<string> { "واحد پیشفرض قابل ویرایش نیست" }
                });
            var repetitive = await unitRepository.GetByName(unit.Name, cancellationToken);
            if (repetitive != null && repetitive.Id != unit.Id)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "نام واحد تکراری است" }
                });
            if (repetitive != null) unitRepository.Detach(repetitive);
            await unitRepository.UpdateAsync(unit, cancellationToken);
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
            if (id == 1)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest,
                    Messages = new List<string> { "واحد پیشفرض قابل حذف نیست" }
                });
            await unitRepository.DeleteAsync(id, cancellationToken);
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

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> ConvertHolooToSunflower(CancellationToken cancellationToken)
    {
        try
        {
            var units = (await holooUnitRepository.GetAll(cancellationToken))!.Select(x => new Unit
            {
                Name = x.Unit_Name,
                Few = x.Unit_Few,
                UnitCode = x.Unit_Code,
                assay = x.Ayar,
                UnitWeight = x.Vahed_Vazn
            });

            try
            {
                await unitRepository.AddRangeAsync(units, cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogCritical(e, e.Message);
                return Ok(new ApiResult
                {
                    Code = ResultCode.DatabaseError,
                    Messages = new List<string> { "افزودن اتوماتیک به مشکل برخورد کرد" }
                });
            }

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