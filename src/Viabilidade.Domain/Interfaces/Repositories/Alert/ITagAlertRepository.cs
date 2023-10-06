using Viabilidade.Domain.Entities.Alert;

namespace Viabilidade.Domain.Interfaces.Repositories.Alert
{
    public interface ITagAlertRepository : IBaseRepository<TagAlertEntity>
    {
        Task<TagAlertEntity> CreateAsync(TagAlertEntity entity);
        Task<bool> DeleteByRuleAsync(int ruleId);
        Task<IEnumerable<TagAlertEntity>> GetByRuleAsync(int ruleId, bool? active = null);
        Task<IEnumerable<TagAlertEntity>> GetByTagAsync(int tagId, bool? active = null);
    }
}
