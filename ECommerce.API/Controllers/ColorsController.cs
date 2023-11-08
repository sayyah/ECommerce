namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ColorsController : ControllerBase
{
    private readonly IColorRepository _colorRepository;
    private readonly ILogger<ColorsController> _logger;
    private readonly IColorDtoMapper _colorMapper;

    public ColorsController(IColorRepository colorGroupRepository, ILogger<ColorsController> logger ,
                            IColorDtoMapper colorMapper)
    {
        _colorRepository = colorGroupRepository;
        _logger = logger;
        _colorMapper = colorMapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var colors = await _colorRepository.GetAll(cancellationToken);
            IEnumerable<ColorReadDto> colorsRead = new List<ColorReadDto>();
            colorsRead = _colorMapper.CreateMapper(colors, colorsRead);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = colorsRead
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWithPagination([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var pagedListColorEntity = await _colorRepository.Search(paginationParameters, cancellationToken);
            IEnumerable<ColorReadDto> colorReadDtoList = new List<ColorReadDto>();
            colorReadDtoList = _colorMapper.CreateMapper(pagedListColorEntity.ToList(), colorReadDtoList);

            var paginationDetails = new PaginationDetails
            {
                TotalCount = pagedListColorEntity.TotalCount,
                PageSize = pagedListColorEntity.PageSize,
                CurrentPage = pagedListColorEntity.CurrentPage,
                TotalPages = pagedListColorEntity.TotalPages,
                HasNext = pagedListColorEntity.HasNext,
                HasPrevious = pagedListColorEntity.HasPrevious,
                Search = paginationParameters.Search
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
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpGet]
    public async Task<ActionResult<ColorReadDto>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _colorRepository.GetByIdAsync(cancellationToken, id);
            ColorReadDto colorReadDto = new();
            colorReadDto = _colorMapper.CreateMapper(result, colorReadDto);
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
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Post(Color color, CancellationToken cancellationToken)
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

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await _colorRepository.AddAsync(color, cancellationToken)
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
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
            await _colorRepository.UpdateAsync(color, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
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
            await _colorRepository.DeleteAsync(id, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
                { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }
}