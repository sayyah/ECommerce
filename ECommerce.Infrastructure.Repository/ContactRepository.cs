using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class ContactRepository(SunflowerECommerceDbContext context) : RepositoryBase<Contact>(context),
    IContactRepository
{
    public async Task<Contact?> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Contacts.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Contact?> GetByEmail(string email, CancellationToken cancellationToken)
    {
        return await context.Contacts.Where(x => x.Email == email).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Contact?> GetRepetitive(Contact contact, CancellationToken cancellationToken)
    {
        return await context.Contacts
            .Where(x => x.Email == contact.Email
                        && x.Name == contact.Name
                        && x.Subject == contact.Subject
                        && x.Message == contact.Message)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public PagedList<Contact> Search(PaginationParameters paginationParameters)
    {
        return PagedList<Contact>.ToPagedList(
           context.Contacts.Where(x => x.Email.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
