using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Areas.Admin.Pages.Contacts;

public class IndexModel : PageModel
{
    private readonly IContactService _contactService;

    public IndexModel(IContactService contactService)
    {
        _contactService = contactService;
    }

    public ServiceResult<List<Contact>> Contacts { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task OnGet(string search = "", int pageNumber = 1, int pageSize = 10, string message = null,
        string code = null)
    {
        Message = message;
        Code = code;
        var result = await _contactService.GetAll(search, pageNumber, pageSize);
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