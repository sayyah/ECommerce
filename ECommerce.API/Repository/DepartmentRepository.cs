﻿using API.DataContext;
using API.Interface;
using Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Utilities;
using Entities.Helper;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class DepartmentRepository : AsyncRepository<Department>, IDepartmentRepository
    {
        private readonly SunflowerECommerceDbContext _context;
        public DepartmentRepository(SunflowerECommerceDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<PagedList<Department>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken)
        {
            return PagedList<Department>.ToPagedList(await _context.Departments.Where(x => x.Title.Contains(paginationParameters.Search)).AsNoTracking().OrderBy(on => on.Id).ToListAsync(cancellationToken),
                paginationParameters.PageNumber,
                paginationParameters.PageSize);
        }
        public async Task<Department> GetByTitle(string name, CancellationToken cancellationToken) => await _context.Departments.Where(x => x.Title == name).FirstOrDefaultAsync(cancellationToken);

    }
}
