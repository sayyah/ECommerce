using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Pages;

public class ContactModel(IContactService contactService) : PageModel
{
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
            Contact.CreatedDate = DateTime.Now;
            Contact.UpdatedDate = DateTime.Now;
            var result = await contactService.Add(Contact);
            Message = result.Message;
            Code = result.Code.ToString();
            if (result.Code != 0) return Page();
            ModelState.Clear();
            return Page();
        }

        return Page();
    }
}