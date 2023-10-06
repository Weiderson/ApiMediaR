using Viabilidade.Domain.Entities.Alert;

namespace Viabilidade.Domain.Interfaces.Repositories.Alert
{
    public interface IFavoriteAlertRepository
    {
        Task<FavoriteAlertEntity> GetAsync(int id);
        Task<FavoriteAlertEntity> GetByRuleUserAsync(int ruleId);
        Task<bool> ExistFavoriteAsync(int ruleId);
        Task<FavoriteAlertEntity> CreateAsync(FavoriteAlertEntity entity);
        Task<FavoriteAlertEntity> UpdateAsync(int id, FavoriteAlertEntity entity);
        Task<bool> DeleteAsync(int id);
    }
}
