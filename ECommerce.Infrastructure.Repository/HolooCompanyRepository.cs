<<<<<<< HEAD
﻿using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;
=======
﻿namespace ECommerce.Infrastructure.Repository;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

public class HolooCompanyRepository : AsyncRepository<HolooCompany>, IHolooCompanyRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public HolooCompanyRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }
}
