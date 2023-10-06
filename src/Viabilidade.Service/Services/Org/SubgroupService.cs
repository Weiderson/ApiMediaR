using AutoMapper;
using Viabilidade.Domain.Interfaces.Cache;
using Viabilidade.Domain.Interfaces.Repositories.Org;
using Viabilidade.Domain.Interfaces.Services.Org;
using Viabilidade.Domain.Models.Org;

namespace Viabilidade.Service.Services.Org
{
    public class SubgroupService : ISubgroupService
    {
        private readonly ISubgroupRepository _tipoSubGrupoRepository;
        private readonly IStorageCache _cache;
        private readonly IMapper _mapper;
        public SubgroupService(ISubgroupRepository tipoSubGrupoRepository, IStorageCache cache, IMapper mapper)
        {
            _tipoSubGrupoRepository = tipoSubGrupoRepository;
            _cache = cache;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SubgroupModel>> GetAsync(bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("Subgroup", () => _tipoSubGrupoRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<SubgroupModel>>(entity);
            return _mapper.Map<IEnumerable<SubgroupModel>>(entity.Where(x => x.Active == active));
        }

        public async Task<SubgroupModel> GetAsync(int id)
        {
            var entity = await _tipoSubGrupoRepository.GetAsync(id);
            return _mapper.Map<SubgroupModel>(entity);
        }
    }
}
