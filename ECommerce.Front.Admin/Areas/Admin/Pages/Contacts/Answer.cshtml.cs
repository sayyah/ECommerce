using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Contacts;

public class AnswerModel : PageModel
{
    private readonly IContactService _contactService;

    public AnswerModel(IContactService contactService)
    {
        _contactService = contactService;
    }

    [BindProperty] public Contact Contact { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }


    public async Task<IActionResult> OnGet(int id)
    {
        var result = await Initial(id);
        if (result.Code == 0) return Page();
        return RedirectToPage("/Contacts/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var result = await _contactService.Edit(Contact);
            if (result.Code == 0)
                return RedirectToPage("/Contacts/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });

            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        await Initial(Contact.Id);
        return Page();
    }

    private async Task<ServiceResult<Contact>> Initial(int id)
    {
        var result = await _contactService.GetById(id);
        if (result.Code > 0)
            return new ServiceResult<Contact>
            {
                Code = result.Code,
                Message = result.Message
            };
        Contact = result.ReturnData;

        return result;
    }
}