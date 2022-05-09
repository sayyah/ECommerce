﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Entities.Helper;

namespace Services.IServices
{
    public interface IBlogService : IEntityService<Blog>
    {
        Task<ServiceResult<List<Blog>>> Filtering(string filter);
        Task<ServiceResult<List<Blog>>> Load(string search = "", int pageNumber = 0, int pageSize = 10);
        Task<ServiceResult<Dictionary<int, string>>> LoadDictionary();
        Task<ServiceResult> Add(Blog blog);
        Task<ServiceResult> Edit(Blog blog);
        Task<ServiceResult> Delete(int id);
        Task<ServiceResult<Blog>> GetById(int id);
    }
}