using AutoMapper;
using Viabilidade.Domain.Interfaces.Cache;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Models.Alert;

namespace Viabilidade.Service.Services.Alert
{
    public class AlgorithmService : IAlgorithmService
    {
        private readonly IAlgorithmRepository _algoritmoTipoRepository;
        private readonly IStorageCache _cache;
        private readonly IMapper _mapper;
        public AlgorithmService(IAlgorithmRepository algoritmoTipoRepository, IStorageCache cache,IMapper mapper)
        {
            _algoritmoTipoRepository = algoritmoTipoRepository;
            _cache = cache;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AlgorithmModel>> GetAsync(bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("Algorithm", () => _algoritmoTipoRepository.GetAsync());
            if(active == null)
                return _mapper.Map<IEnumerable<AlgorithmModel>>(entity);
            return _mapper.Map<IEnumerable<AlgorithmModel>>(entity.Where(x => x.Active == active));
        }

        public async Task<AlgorithmModel> GetAsync(int id)
        {
            var entity = await _algoritmoTipoRepository.GetAsync(id);
            return _mapper.Map<AlgorithmModel>(entity);
        }
    }
}
