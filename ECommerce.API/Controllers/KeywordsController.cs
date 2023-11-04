﻿using ECommerce.API.Interface;
using Ecommerce.Entities;
using Ecommerce.Entities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class KeywordsController : ControllerBase
{
    private readonly IKeywordRepository _keywordRepository;
    private readonly ILogger<KeywordsController> _logger;

    public KeywordsController(IKeywordRepository keywordRepository, ILogger<KeywordsController> logger)
    {
        _keywordRepository = keywordRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        
            if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
            var entity = await _keywordRepository.Search(paginationParameters, cancellationToken);
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
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await _keywordRepository.GetAll(cancellationToken)
            });
    }

    [HttpGet]
    public async Task<IActionResult> GetKeywordsByProductId(int id, CancellationToken cancellationToken)
    {
        
            var keywordList = await _keywordRepository.GetByProductId(id, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = keywordList
            });
    }

    [HttpGet]
    public async Task<ActionResult<Keyword>> GetById(int id, CancellationToken cancellationToken)
    {
        
            var result = await _keywordRepository.GetByIdAsync(cancellationToken, id);
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
    public async Task<IActionResult> Post(Keyword keywords, CancellationToken cancellationToken)
    {
        
            if (keywords == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });
            keywords.KeywordText = keywords.KeywordText.Trim();

            var repetitiveCategory = await _keywordRepository.GetByKeywordText(keywords.KeywordText, cancellationToken);
            if (repetitiveCategory != null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> {"کلمه کلیدی تکراری است"}
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await _keywordRepository.AddAsync(keywords, cancellationToken)
            });
    }

    [HttpPut]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> Put(Keyword keyword, CancellationToken cancellationToken)
    {
        
            var repetitive = await _keywordRepository.GetByKeywordText(keyword.KeywordText, cancellationToken);
            if (repetitive != null && repetitive.Id != keyword.Id)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> {"کلمه کلیدی تکراری است"}
                });
            if (repetitive != null) _keywordRepository.Detach(repetitive);
            await _keywordRepository.UpdateAsync(keyword, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
    }

    [HttpDelete]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        
            await _keywordRepository.DeleteAsync(id, cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
    }
}