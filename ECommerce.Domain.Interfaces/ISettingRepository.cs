using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface ISettingRepository : IAsyncRepository<Setting>
{
    string IsDollar();
}
