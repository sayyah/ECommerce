using System.Globalization;
using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Contacts;

public class IndexModel(IContactService contactService) : PageModel
{
    public ServiceResult<List<Contact>> Contacts { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task OnGet(string search = "", int pageNumber = 1, int pageSize = 10, string message = null,
        string code = null)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("fa-IR");
        Message = message;
        Code = code;
        var result = await contactService.GetAll(search, pageNumber, pageSize);
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

            Contacts = result;
        }
    }
}