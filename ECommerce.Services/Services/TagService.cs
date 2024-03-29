﻿using ECommerce.Application.ViewModels;

namespace ECommerce.Services.Services;

public class TagService(IHttpService http, IEntityService<TagProductId> tagViewModelEntityService)
    : EntityService<Tag>(http), ITagService
{
    private const string Url = "api/Tags";
    private List<Tag> _tags;

    public async Task<ServiceResult<List<Tag>>> Load(string search = "", int pageNumber = 0, int pageSize = 10)
    {
        var result = await ReadList(Url, $"Get?PageNumber={pageNumber}&PageSize={pageSize}&Search={search}");
        return Return(result);
    }

    public async Task<ServiceResult<List<Tag>>> GetAll()
    {
        var result = await ReadList(Url, "GetAll");
        return Return(result);
    }

    public async Task<ServiceResult<Dictionary<int, string>>> LoadDictionary()
    {
        var result = await ReadList(Url);
        if (result.Code == ResultCode.Success)
            return new ServiceResult<Dictionary<int, string>>
            {
                Code = ServiceCode.Success,
                ReturnData = result.ReturnData.ToDictionary(item => item.Id, item => item.TagText),
                Message = result.Messages?.FirstOrDefault()
            };
        return new ServiceResult<Dictionary<int, string>>
        {
            Code = ServiceCode.Error,
            Message = result.GetBody()
        };
    }

    public async Task<ServiceResult<List<Tag>>> Filtering(string filter)
    {
        if (_tags == null)
        {
            var tags = await Load();
            if (tags.Code > 0) return tags;
            _tags = tags.ReturnData;
        }

        var result = _tags.Where(x => x.TagText.Contains(filter)).ToList();
        if (result.Count == 0)
            return new ServiceResult<List<Tag>> { Code = ServiceCode.Info, Message = "تگی یافت نشد" };
        return new ServiceResult<List<Tag>>
        {
            Code = ServiceCode.Success,
            ReturnData = result
        };
    }

    public async Task<ServiceResult> Add(Tag tag)
    {
        var result = await Create(Url, tag);
        _tags = null;
        return Return(result);
    }

    public async Task<ServiceResult> Edit(Tag tag)
    {
        var result = await Update(Url, tag);
        _tags = null;
        return Return(result);
    }

    public async Task<ServiceResult> Delete(int id)
    {
        //var result = await Delete(Url, id);
        //_tags = null;
        //return Return(result);
        var result = await http.DeleteAsync(Url, id);
        _tags = null;
        if (result.Code == ResultCode.Success)
            return new ServiceResult
            {
                Code = ServiceCode.Success,
                Message = "با موفقیت حذف شد"
            };
        return new ServiceResult
            { Code = ServiceCode.Error, Message = "به علت وابستگی با عناصر دیگر امکان حذف وجود ندارد" };
    }

    public async Task<ServiceResult<List<TagProductId>>> GetTagsByProductId(int productId)
    {
        var result = await tagViewModelEntityService.ReadList(Url, $"GetByProductId?id={productId}");
        return Return(result);
    }

    public async Task<ServiceResult<Tag>> GetById(int id)
    {
        var result = await http.GetAsync<Tag>(Url, $"GetById?id={id}");
        return Return(result);
    }

    public async Task<ServiceResult<Tag>> GetByTagText(string TagText)
    {
        var result = await http.GetAsync<Tag>(Url, $"GetByTagText={TagText}");
        return Return(result);
    }

    public async Task<ServiceResult<List<Tag>>> GetAllProductTags()
    {
        var result = await ReadList(Url, "GetAllProductTags");
        return Return(result);
    }

    public async Task<ServiceResult<List<Tag>>> GetAllBlogTags()
    {
        var result = await ReadList(Url, "GetAllBlogTags");
        return Return(result);
    }
}