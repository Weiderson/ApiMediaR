using Viabilidade.Domain.DTO.Alert;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Models.QueryParams.Alert;

namespace Viabilidade.Domain.Interfaces.Repositories.Alert
{
    public interface IAlertRepository
    {
        Task<AlertEntity> CreateAsync(AlertEntity entity);
        Task<IEnumerable<AlertEntity>> GetAsync();
        Task<IEnumerable<AlertEntity>> GetByEntityRuleAsync(int entityRuleId, bool? treated);
        Task<AlertEntity> GetLastByEntityRuleAsync(int entityRuleId);
        Task<IEnumerable<Tuple<AlertGroupDto, int>>> GroupAsync(AlertQueryParams queryParams);
        Task<AlertEntity> GetAsync(int id);
        Task<AlertEntity> PreviewAsync(int id);
        Task<IEnumerable<AlertEntity>> GetAsync(IEnumerable<int> ids);
        Task<AlertEntity> UpdateAsync(int id, AlertEntity entity);
        Task<bool> DeleteAsync(int id);
        Task<int> CountLowSeverityByEntityRuleAsync(int entityRuleId);
    }
}
