using Viabilidade.Domain.Entities.Alert;

namespace Viabilidade.Domain.Interfaces.Repositories.Alert
{
    public interface ITreatmentTypeRepository : IBaseRepository<TreatmentTypeEntity>
    {
        Task<IEnumerable<TreatmentTypeEntity>> GetByTreatmentClassAsync(int treatmentClassId, bool? active = null);
    }
}
