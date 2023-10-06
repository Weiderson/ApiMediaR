using Viabilidade.Domain.DTO.Rule;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Models.QueryParams.Rule;

namespace Viabilidade.Domain.Interfaces.Repositories.Alert
{
    public interface IRuleRepository
    {
        Task<RuleEntity> CreateAsync(RuleEntity entity);
        Task<IEnumerable<RuleEntity>> GetAsync();
        Task<IEnumerable<Tuple<RuleGroupDto, int>>> GroupByRuleAsync(RuleQueryParams queryParams);
        Task<IEnumerable<Tuple<RuleGroupDto, int>>> GroupByEntityAsync(RuleQueryParams queryParams);
        Task<RuleEntity> GetAsync(int id);
        Task<RuleEntity> PreviewAsync(int id);
        Task<RuleEntity> UpdateAsync(int id, RuleEntity entity);
        Task<bool> DeleteAsync(int id);
    }
}
