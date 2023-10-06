using Viabilidade.Domain.DTO.EntityRule;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Models.QueryParams;
using Viabilidade.Domain.Models.QueryParams.EntityRule;

namespace Viabilidade.Domain.Interfaces.Repositories.Alert
{
    public interface IEntityRuleRepository
    {
        Task<EntityRuleEntity> CreateAsync(EntityRuleEntity entity);
        Task<EntityRuleEntity> UpdateAsync(int id, EntityRuleEntity entity);
        Task<EntityRuleEntity> GetAsync(int id);
        Task<EntityRuleEntity> PreviewAsync(int id);
        Task<EntityRuleEntity> PreviewAsync(int ruleId, int entityId);
        Task<IEnumerable<EntityRuleEntity>> GetByRuleAsync(int ruleId);
        Task<IEnumerable<EntityRuleEntity>> GetByEntityAsync(int entityId);
        Task<IEnumerable<Tuple<EntityRuleGroupByRuleDto, int>>> GroupByRuleAsync(int ruleId, EntityRuleQueryParams queryParams);
        Task<IEnumerable<Tuple<EntityRuleGroupByEntityDto, int>>> GroupByEntityAsync(int entityId, EntityRuleQueryParams queryParams);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByRuleAsync(int ruleId);
    }
}
