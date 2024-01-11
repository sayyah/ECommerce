using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UnitsController(IUnitOfWork unitOfWork, ILogger<UnitsController> logger) : ControllerBase
{
    private readonly IHolooUnitRepository _holooUnitRepository = unitOfWork.GetHolooRepository<HolooUnitRepository, HolooUnit>();
    private readonly IUnitRepository _unitRepository = unitOfWork.GetRepository<UnitRepository, Unit>();

    [HttpGet]
    public IActionResult Get([FromQuery] PaginationParameters paginationParameters)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity =  _unitRepository.Search(paginationParameters);
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
                ReturnData = await _holooUnitRepository.GetAll(cancellationToken)
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
            var result = await _unitRepository.GetByIdAsync(cancellationToken, id);
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
    public async Task<IActionResult> Post(Unit? unit, CancellationToken cancellationToken)
    {
        try
        {
            if (unit == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });
            unit.Name = unit.Name.Trim();

            var repetitiveUnit = await _unitRepository.GetByName(unit.Name, cancellationToken);
            if (repetitiveUnit != null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "نام واحد تکراری است" }
                });

            _unitRepository.Add(unit);
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
            var repetitive = await _unitRepository.GetByName(unit.Name, cancellationToken);
            if (repetitive != null && repetitive.Id != unit.Id)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "نام واحد تکراری است" }
                });
            if (repetitive != null) _unitRepository.Detach(repetitive);
            _unitRepository.Update(unit);
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
            if (id == 1)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest,
                    Messages = new List<string> { "واحد پیشفرض قابل حذف نیست" }
                });
            await _unitRepository.DeleteById(id, cancellationToken);
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

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> ConvertHolooToSunflower(CancellationToken cancellationToken)
    {
        try
        {
            var units = (await _holooUnitRepository.GetAll(cancellationToken)).Select(x => new Unit
            {
                Name = x.Unit_Name,
                Few = x.Unit_Few,
                UnitCode = x.Unit_Code,
                assay = x.Ayar,
                UnitWeight = x.Vahed_Vazn
            });

            try
            {
                _unitRepository.AddRange(units);
                await unitOfWork.SaveAsync(cancellationToken);
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