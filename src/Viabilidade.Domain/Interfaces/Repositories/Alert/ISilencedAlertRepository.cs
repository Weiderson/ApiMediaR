using Viabilidade.Domain.Entities.Alert;

namespace Viabilidade.Domain.Interfaces.Repositories.Alert
{
    public interface ISilencedAlertRepository
    {
        Task<SilencedAlertEntity> CreateAsync(SilencedAlertEntity entity);
    }
}
