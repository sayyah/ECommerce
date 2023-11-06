<<<<<<< HEAD
﻿using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;
=======
﻿namespace ECommerce.Infrastructure.Repository;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

public class SettingRepository : AsyncRepository<Setting>, ISettingRepository
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
