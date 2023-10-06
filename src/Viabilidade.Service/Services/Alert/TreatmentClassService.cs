using AutoMapper;
using Viabilidade.Domain.Interfaces.Cache;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Models.Alert;

namespace Viabilidade.Service.Services.Alert
{
    public class TreatmentClassService : ITreatmentClassService
    {
        private readonly ITreatmentClassRepository _classeTratativaRepository;
        private readonly IStorageCache _cache;
        private readonly IMapper _mapper;
        public TreatmentClassService(ITreatmentClassRepository classeTratativaRepository, IStorageCache cache,IMapper mapper)
        {
            _classeTratativaRepository = classeTratativaRepository;
            _cache = cache;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TreatmentClassModel>> GetAsync(bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("TreatmentClass", () => _classeTratativaRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<TreatmentClassModel>>(entity);
            return _mapper.Map<IEnumerable<TreatmentClassModel>>(entity.Where(x => x.Active == active));
        }

        public async Task<TreatmentClassModel> GetAsync(int id)
        {
            var entity = await _classeTratativaRepository.GetAsync(id);
            return _mapper.Map<TreatmentClassModel>(entity);
        }
    }
}
