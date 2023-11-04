﻿using ECommerce.API.Interface;
using Ecommerce.Entities;
using Ecommerce.Entities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class StoreController : ControllerBase
{
    private readonly ILogger<StoreController> _logger;

    private readonly IStoreRepository _storeRepository;

    public StoreController(IStoreRepository storeRepository, ILogger<StoreController> logger)
    {
        _storeRepository = storeRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity = await _storeRepository.Search(paginationParameters, cancellationToken);
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
    public async Task<ActionResult<Store>> GetById(int id, CancellationToken cancellationToken)
    {
        
            var result = await _storeRepository.GetByIdAsync(cancellationToken, id);
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
    public async Task<IActionResult> Post(Store store, CancellationToken cancellationToken)
    {
        
            if (store == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });
            store.Name = store.Name.Trim();

            var repetitiveStore = await _storeRepository.GetByName(store.Name, cancellationToken);
            if (repetitiveStore != null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> {"نام انبار تکراری است"}
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await _storeRepository.AddAsync(store, cancellationToken)
            });
    }

    [HttpPut]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> Put(Store store, CancellationToken cancellationToken)
    {
        
            var repetitive = await _storeRepository.GetByName(store.Name, cancellationToken);
            if (repetitive != null && repetitive.Id != store.Id)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> {"نام انبار تکراری است"}
                });
            if (repetitive != null) _storeRepository.Detach(repetitive);
            await _storeRepository.UpdateAsync(store, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
    }

    [HttpDelete]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        
            await _storeRepository.DeleteAsync(id, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
    }
}