﻿using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class SettingRepository : RepositoryBase<Setting>, ISettingRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public SettingRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public string IsDollar()
    {
        return _context.Settings.First(x => x.Name.Equals("Currency")).Value;
    }
}
