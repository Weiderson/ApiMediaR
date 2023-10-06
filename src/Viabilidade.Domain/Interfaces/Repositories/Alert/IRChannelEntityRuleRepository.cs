using Viabilidade.Domain.Entities.Alert;

namespace Viabilidade.Domain.Interfaces.Repositories.Alert
{
    public interface IRChannelEntityRuleRepository
    {
        Task<RChannelEntityRuleEntity> CreateAsync(RChannelEntityRuleEntity entity);

        Task<bool> DeleteByEntityRuleAsync(IEnumerable<int> entityRuleIds);
    }
}
