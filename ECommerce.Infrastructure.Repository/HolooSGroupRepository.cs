﻿using ECommerce.Domain.Entities.HolooEntity;
using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class HolooSGroupRepository : HolooRepository<HolooSGroup>, IHolooSGroupRepository
{
    private readonly HolooDbContext _context;

    public HolooSGroupRepository(HolooDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HolooSGroup>> GetSGroupByMCode(string mCode, CancellationToken cancellationToken)
    {
        return await _context.S_GROUP.Where(x => x.M_groupcode == mCode).ToListAsync(cancellationToken);
    }
}
