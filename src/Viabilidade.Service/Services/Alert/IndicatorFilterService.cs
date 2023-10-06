using AutoMapper;
using Viabilidade.Domain.Interfaces.Cache;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Models.Alert;

namespace Viabilidade.Service.Services.Alert
{
    public class IndicatorFilterService : IIndicatorFilterService
    {

        private readonly IIndicatorFilterRepository _filtroIndicadorRepository;
        private readonly IStorageCache _cache;
        private readonly IMapper _mapper;
        public IndicatorFilterService(IIndicatorFilterRepository filtroIndicadorRepository, IStorageCache cache, IMapper mapper)
        {
            _filtroIndicadorRepository = filtroIndicadorRepository;
            _cache = cache;
            _mapper = mapper;
        }
        public async Task<IEnumerable<IndicatorFilterModel>> GetAsync(bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("IndicatorFilter", () => _filtroIndicadorRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<IndicatorFilterModel>>(entity);
            return _mapper.Map<IEnumerable<IndicatorFilterModel>>(entity.Where(x => x.Active == active));
        }

        public async Task<IndicatorFilterModel> GetAsync(int id)
        {
            var entity = await _filtroIndicadorRepository.GetAsync(id);
            return _mapper.Map<IndicatorFilterModel>(entity);
        }
    }
}
