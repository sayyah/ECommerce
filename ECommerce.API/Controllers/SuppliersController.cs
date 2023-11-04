﻿using ECommerce.API.Interface;
using Ecommerce.Entities;
using Ecommerce.Entities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
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
    public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
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

    [HttpGet]
    public async Task<ActionResult<Supplier>> GetById(int id, CancellationToken cancellationToken)
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

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Post(Supplier supplier, CancellationToken cancellationToken)
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

    [HttpPut]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> Put(Supplier supplier, CancellationToken cancellationToken)
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

    [HttpDelete]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        
            await _supplierRepository.DeleteAsync(id, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
    }
}