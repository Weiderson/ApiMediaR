using Viabilidade.Domain.Entities.Alert;

namespace Viabilidade.Domain.Interfaces.Repositories.Alert
{
    public interface IAttachmentRepository
    {

        Task<AttachmentEntity> CreateAsync(AttachmentEntity model);
        Task<AttachmentEntity> GetAsync(int id);
        Task<IEnumerable<AttachmentEntity>> GetByTreatmentAsync(int treatmentId);
    }
}
