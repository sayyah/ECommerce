using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Cities;

public class IndexModel(ICityService cityService) : PageModel
{
    public ServiceResult<List<City>> Cities { get; set; }

    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(string search = "", int pageNumber = 1, int pageSize = 10,
        string message = null, string code = null)
    {
        Message = message;
        Code = code;
        var result = await cityService.GetWithPagination(search, pageNumber, pageSize);
        if (result.Code == ServiceCode.Success)
        {
            if (Message != null)
            {
                Message = Message;
                Code = Code;
            }
            else
            {
                Message = result.Message;
                Code = result.Code.ToString();
            }

            Cities = result;
            return Page();
        }

        return RedirectToPage("/index", new { message = result.Message, code = result.Code.ToString() });
    }
}