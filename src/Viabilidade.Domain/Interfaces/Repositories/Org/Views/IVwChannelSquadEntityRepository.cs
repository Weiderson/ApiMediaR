using Viabilidade.Domain.Entities.Views;

namespace Viabilidade.Domain.Interfaces.Repositories.Org.Views
{
    public interface IVwChannelSquadEntityRepository
    {
        Task<IEnumerable<BondEntity>> GetAsync(string search, int segmentId);
    }
}
