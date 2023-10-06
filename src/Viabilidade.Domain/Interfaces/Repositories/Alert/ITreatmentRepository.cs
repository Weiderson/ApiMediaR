using Viabilidade.Domain.DTO.Treatment;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Models.QueryParams.Treatment;

namespace Viabilidade.Domain.Interfaces.Repositories.Alert
{
    public interface ITreatmentRepository
    {
        Task<TreatmentEntity> CreateAsync(TreatmentEntity entity);
        Task<IEnumerable<Tuple<TreatmentGroupDto, int>>> GroupAsync(TreatmentQueryParams queryParams);
        Task<TreatmentEntity> PreviewAsync(int id);
        Task<TreatmentEntity> PreviewDetailAsync(int id);
        Task<IEnumerable<TreatmentByEntityRuleGroupDto>> GetByEntityRuleGroupAsync(int entityRuleId);
        Task<TreatmentEntity> GetAsync(int id);
        Task<int> CountByEntityRuleWasProblemGroupAsync(int entityRuleId);
    }
}
