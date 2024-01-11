using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface ISettingRepository : IRepositoryBase<Setting>
{
    string IsDollar();
}
