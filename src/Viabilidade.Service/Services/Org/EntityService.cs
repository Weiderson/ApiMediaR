using AutoMapper;
using Viabilidade.Domain.Interfaces.Cache;
using Viabilidade.Domain.Interfaces.Repositories.Org;
using Viabilidade.Domain.Interfaces.Services.Org;
using Viabilidade.Domain.Models.Org;

namespace Viabilidade.Service.Services.Org
{
    public class EntityService : IEntityService
    {
        private readonly IEntityRepository _entidadeRepository;
        private readonly IStorageCache _cache;
        private readonly IMapper _mapper;
        public EntityService(IEntityRepository entidadeRepository, IStorageCache cache,IMapper mapper)
        {
            _entidadeRepository = entidadeRepository;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EntityModel>> GetAllFilter(int? id, string name, string originalEntityId)
        {
            var entity = await _entidadeRepository.GetAllFilter(id, name, originalEntityId);
            return _mapper.Map<IEnumerable<EntityModel>>(entity);
        }

        public async Task<IEnumerable<EntityModel>> GetBySegmentSquadAsync(int squadId, int segmentId)
        {
            var entity = await _entidadeRepository.GetBySegmentSquadAsync(squadId, segmentId);
            return _mapper.Map<IEnumerable<EntityModel>>(entity);
        }

        public async Task<IEnumerable<EntityModel>> GetAsync(bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("Entity", () => _entidadeRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<EntityModel>>(entity);
            return _mapper.Map<IEnumerable<EntityModel>>(entity.Where(x => x.Active == active));
        }

        public async Task<EntityModel> GetAsync(int id)
        {
            var entity = await _entidadeRepository.GetAsync(id);
            return _mapper.Map<EntityModel>(entity);
        }

        public async Task<EntityModel> GetByOriginalEntityAsync(int originalEntityId)
        {
            var entity = await _entidadeRepository.GetByOriginalEntityAsync(originalEntityId);
            return _mapper.Map<EntityModel>(entity);
        }
       
    }
}
