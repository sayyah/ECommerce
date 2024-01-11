namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class DepartmentsController(IUnitOfWork unitOfWork, ILogger<DepartmentsController> logger)
    : ControllerBase
{
    private readonly IDepartmentRepository _departmentRepository = unitOfWork.GetRepository<DepartmentRepository, Department>();

    [HttpGet]
    public IActionResult Get([FromQuery] PaginationParameters paginationParameters)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity =  _departmentRepository.Search(paginationParameters);
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
    public async Task<ActionResult<Department>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _departmentRepository.GetByIdAsync(cancellationToken, id);
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
    public async Task<ActionResult<Department>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _departmentRepository.GetAllAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = result
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
    public async Task<IActionResult> Post(Department? department, CancellationToken cancellationToken)
    {
        try
        {
            if (department == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });
            department.Title = department.Title.Trim();

            var repetitiveDepartment = await _departmentRepository.GetByTitle(department.Title, cancellationToken);
            if (repetitiveDepartment != null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "عنوان دپارتمان تکراری است" }
                });

            _departmentRepository.Add(department);
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
    public async Task<ActionResult<bool>> Put(Department department, CancellationToken cancellationToken)
    {
        try
        {
            var repetitiveDepartment = await _departmentRepository.GetByTitle(department.Title, cancellationToken);
            if (repetitiveDepartment != null && repetitiveDepartment.Id != department.Id)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { "نام دپارتمان تکراری است" }
                });
            if (repetitiveDepartment != null) _departmentRepository.Detach(repetitiveDepartment);
            _departmentRepository.Update(department);
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
            await _departmentRepository.DeleteById(id, cancellationToken);
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
}