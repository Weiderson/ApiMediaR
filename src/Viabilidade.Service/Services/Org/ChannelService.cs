using AutoMapper;
using System;
using Viabilidade.Domain.Interfaces.Cache;
using Viabilidade.Domain.Interfaces.Repositories.Org;
using Viabilidade.Domain.Interfaces.Services.Org;
using Viabilidade.Domain.Models.Org;

namespace Viabilidade.Service.Services.Org
{
    public class ChannelService : IChannelService
    {

        private readonly IChannelRepository _canalRepository;
        private readonly IStorageCache _cache;
        private readonly IMapper _mapper;
        public ChannelService(IChannelRepository canalRepository, IStorageCache cache, IMapper mapper)
        {
            _canalRepository = canalRepository;
            _cache = cache;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ChannelModel>> GetAsync(bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("Channel", () => _canalRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<ChannelModel>>(entity);
            return _mapper.Map<IEnumerable<ChannelModel>>(entity.Where(x => x.Active == active));
        }

        public async Task<ChannelModel> GetAsync(int id)
        {
            var entity = await _canalRepository.GetAsync(id);
            return _mapper.Map<ChannelModel>(entity);
        }

        public async Task<IEnumerable<ChannelModel>> GetBySubgroupAsync(int subgroupId, bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("Channel", () => _canalRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<ChannelModel>>(entity.Where(x => x.SubgroupId == subgroupId));
            return _mapper.Map<IEnumerable<ChannelModel>>(entity.Where(x => x.SubgroupId == subgroupId && x.Active == active));
        }
    }
}
