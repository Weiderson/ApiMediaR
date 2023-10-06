using Viabilidade.Domain.Entities.Alert;

namespace Viabilidade.Domain.Interfaces.Repositories.Alert
{
    public interface IIndicatorRepository : IBaseRepository<IndicatorEntity>
    {
        Task<IEnumerable<IndicatorEntity>> GetBySegmentAsync(int segmentId, bool? active = null);
    }
}
