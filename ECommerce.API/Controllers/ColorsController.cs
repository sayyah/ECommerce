using ECommerce.Application.DataTransferObjects.Color;
using ECommerce.Infrastructure.DataTransferObjectMappers;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ColorsController(IUnitOfWork unitOfWork, ILogger<ColorsController> logger, IColorDtoMapper colorMapper) : ControllerBase
{
 
    private readonly IColorRepository _colorRepository = unitOfWork.GetRepository<ColorRepository, Color>();


    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {

            var colors = await _colorRepository.GetAllAsync(cancellationToken);
            IEnumerable<ColorReadDto> colorsRead = new List<ColorReadDto>();
            colorsRead = colorMapper.CreateMapper(colors, colorsRead);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = colorsRead
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWithPagination([FromQuery] ColorsQueryDto query,
                                                          CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(query.PaginationParameters.Search)) query.PaginationParameters.Search = "";
            var pagedListColorEntity = await _colorRepository.Search(query.PaginationParameters, cancellationToken);
            IEnumerable<ColorReadDto> colorReadDtoList = new List<ColorReadDto>();
            colorReadDtoList = colorMapper.CreateMapper(pagedListColorEntity.ToList(), colorReadDtoList);

            var paginationDetails = new PaginationDetails
            {
                TotalCount = pagedListColorEntity.TotalCount,
                PageSize = pagedListColorEntity.PageSize,
                CurrentPage = pagedListColorEntity.CurrentPage,
                TotalPages = pagedListColorEntity.TotalPages,
                HasNext = pagedListColorEntity.HasNext,
                HasPrevious = pagedListColorEntity.HasPrevious,
                Search = query.PaginationParameters.Search
            };
            return Ok(new ApiResult
            {
                PaginationDetails = paginationDetails,
                Code = ResultCode.Success,
                ReturnData = colorReadDtoList
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpGet]
    public async Task<ActionResult<ColorReadDto>> GetById([FromQuery] ColorsQueryDto getColorsQueryDto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _colorRepository.GetByIdAsync(cancellationToken, getColorsQueryDto.Id);
            ColorReadDto colorReadDto = new();
            colorReadDto = colorMapper.CreateMapper(result, colorReadDto);
            if (colorReadDto == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.NotFound
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = colorReadDto
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Post(Color? color, CancellationToken cancellationToken)
    {
        try
        {
            if (color == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });
            color.Name = color.Name.Trim();

            var repetitiveColor = await _colorRepository.GetByName(color.Name, cancellationToken);
            if (repetitiveColor != null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "نام رنگ تکراری است" }
                });

            _colorRepository.Add(color);
            await unitOfWork.SaveAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> Put(Color color, CancellationToken cancellationToken)
    {
        try
        {
            _colorRepository.Update(color);
            await unitOfWork.SaveAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpDelete]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _colorRepository.DeleteById(id, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }
}