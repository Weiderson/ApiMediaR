using AutoMapper;
using Viabilidade.Domain.Interfaces.Cache;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Models.Alert;

namespace Viabilidade.Service.Services.Alert
{
    public class IndicatorService : IIndicatorService
    {

        private readonly IIndicatorRepository _IndicadorRepository;
        private readonly IStorageCache _cache;
        private readonly IMapper _mapper;
        public IndicatorService(IIndicatorRepository IndicadorRepository, IStorageCache cache, IMapper mapper)
        {
            _IndicadorRepository = IndicadorRepository;
            _cache = cache;
            _mapper = mapper;
        }
        public async Task<IEnumerable<IndicatorModel>> GetAsync(bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("Indicator", () => _IndicadorRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<IndicatorModel>>(entity);
            return _mapper.Map<IEnumerable<IndicatorModel>>(entity.Where(x => x.Active == active));
        }

        public async Task<IndicatorModel> GetAsync(int id)
        {
            var entity = await _IndicadorRepository.GetAsync(id);
            return _mapper.Map<IndicatorModel>(entity);
        }

        public async Task<IEnumerable<IndicatorModel>> GetBySegmentAsync(int segmentId, bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("Indicator", () => _IndicadorRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<IndicatorModel>>(entity.Where(x => x.SegmentId == segmentId));
            return _mapper.Map<IEnumerable<IndicatorModel>>(entity.Where(x => x.SegmentId == segmentId && x.Active == active));
        }

       
    }
}
