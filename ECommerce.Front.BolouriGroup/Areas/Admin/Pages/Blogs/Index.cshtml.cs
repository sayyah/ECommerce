using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Entities.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IServices;

namespace ArshaHamrah.Areas.Admin.Pages.Blogs
{
    public class IndexModel : PageModel
    {
        private readonly IBlogService _blogService;

        public IndexModel(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public ServiceResult<List<Blog>> Blogs { get; set; }

        [TempData] public string Message { get; set; }

        [TempData] public string Code { get; set; }

        public async Task<IActionResult> OnGet(string search = "", int pageNumber = 1, int pageSize = 10, string message = null, string code = null)
        {
            Message = message;
            Code = code;
            var result = await _blogService.Load(search,pageNumber, pageSize);
            if (result.Code == ServiceCode.Success)
            {
                Message = result.Message;
                Code = result.Code.ToString();
                Blogs = result;
                return Page();
            }

            return RedirectToPage("/index", new { message = result.Message, code = result.Code.ToString() });
        }
    }
}