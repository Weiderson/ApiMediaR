using Viabilidade.Domain.Entities.Alert;

namespace Viabilidade.Domain.Interfaces.Repositories.Alert
{
    public interface IRTreatmentAlertRepository
    {
        Task<RTreatmentAlertEntity> CreateAsync(RTreatmentAlertEntity entity);
    }
}
