using Viabilidade.Domain.Entities.Org;

namespace Viabilidade.Domain.Interfaces.Repositories.Org
{
    public interface IEntityRepository : IBaseRepository<EntityEntity>
    {
        Task<EntityEntity> GetByOriginalEntityAsync(int originalEntityId);
        Task<IEnumerable<EntityEntity>> GetAllFilter(int? id, string name, string originalEntityId);
        Task<IEnumerable<EntityEntity>> GetBySegmentSquadAsync(int squadId, int segmentId);
    }
}