﻿using ECommerce.API.Interface;
using Ecommerce.Entities;
using Ecommerce.Entities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SuppliersController : ControllerBase
{
    private readonly ILogger<SuppliersController> _logger;

    private readonly ISupplierRepository _supplierRepository;

    public SuppliersController(ISupplierRepository discountRepository, ILogger<SuppliersController> logger)
    {
        _supplierRepository = discountRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromBody] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity = await _supplierRepository.Search(paginationParameters, cancellationToken);
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
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult {Code = ResultCode.DatabaseError});
        }
    }

    [HttpGet("GetById/{id}")]
    public async Task<ActionResult<Supplier>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _supplierRepository.GetByIdAsync(cancellationToken, id);
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
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult {Code = ResultCode.DatabaseError});
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Post(Supplier supplier, CancellationToken cancellationToken)
    {
        try
        {
            if (supplier == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });
            supplier.Name = supplier.Name.Trim();

            var repetitiveSupplier = await _supplierRepository.GetByName(supplier.Name, cancellationToken);
            if (repetitiveSupplier != null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> {"نام تامین کننده تکراری است"}
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await _supplierRepository.AddAsync(supplier, cancellationToken)
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult {Code = ResultCode.DatabaseError});
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> Put(Supplier supplier, CancellationToken cancellationToken)
    {
        try
        {
            var repetitive = await _supplierRepository.GetByName(supplier.Name, cancellationToken);
            if (repetitive != null && repetitive.Id != supplier.Id)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> {"نام تامین کننده تکراری است"}
                });
            if (repetitive != null) _supplierRepository.Detach(repetitive);
            await _supplierRepository.UpdateAsync(supplier, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult {Code = ResultCode.DatabaseError});
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _supplierRepository.DeleteAsync(id, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, e.Message);
            return Ok(new ApiResult {Code = ResultCode.DatabaseError});
        }
    }
}