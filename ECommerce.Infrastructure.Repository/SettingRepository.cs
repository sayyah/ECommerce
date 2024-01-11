namespace ECommerce.Infrastructure.Repository;

public class SettingRepository(SunflowerECommerceDbContext context) : RepositoryBase<Setting>(context),
    ISettingRepository
{
    public string IsDollar()
    {
        return context.Settings.First(x => x.Name.Equals("Currency")).Value;
    }
}
