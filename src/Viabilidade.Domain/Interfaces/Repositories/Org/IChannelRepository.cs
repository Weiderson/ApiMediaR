using Viabilidade.Domain.Entities.Org;

namespace Viabilidade.Domain.Interfaces.Repositories.Org
{
    public interface IChannelRepository: IBaseRepository<ChannelEntity>
    {
        Task<IEnumerable<ChannelEntity>> GetBySubgroupAsync(int subgroupId, bool? active = null);
    }
}
