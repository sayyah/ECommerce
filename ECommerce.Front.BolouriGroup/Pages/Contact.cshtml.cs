using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Pages;

public class ContactModel : PageModel
{
    private readonly IContactService _contactService;

    public ContactModel(IContactService contactService)
    {
        _contactService = contactService;
    }

    [BindProperty] public Contact Contact { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public void OnGet()
    {
        if (Contact == null)
            Contact = new Contact();
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var result = await _contactService.Add(Contact);
            Message = result.Message;
            Code = result.Code.ToString();
            if (result.Code != 0) return Page();
            ModelState.Clear();
            return Page();
        }

        return Page();
    }
}