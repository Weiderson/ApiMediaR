using AutoMapper;
using System;
using Viabilidade.Domain.Interfaces.Cache;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Models.Alert;

namespace Viabilidade.Service.Services.Alert
{
    public class TreatmentTypeService : ITreatmentTypeService
    {

        private readonly ITreatmentTypeRepository _tipoTratativaRepository;
        private readonly IStorageCache _cache;
        private readonly IMapper _mapper;
        public TreatmentTypeService(ITreatmentTypeRepository tipoTratativaRepository, IStorageCache cache, IMapper mapper)
        {
            _tipoTratativaRepository = tipoTratativaRepository;
            _cache = cache;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TreatmentTypeModel>> GetAsync(bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("TreatmentType", () => _tipoTratativaRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<TreatmentTypeModel>>(entity);
            return _mapper.Map<IEnumerable<TreatmentTypeModel>>(entity.Where(x => x.Active == active));
        }

        public async Task<TreatmentTypeModel> GetAsync(int id)
        {
            var entity = await _tipoTratativaRepository.GetAsync(id);
            return _mapper.Map<TreatmentTypeModel>(entity);
        }

        public async Task<IEnumerable<TreatmentTypeModel>> GetByTreatmentClassAsync(int treatmentClassId, bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("TreatmentType", () => _tipoTratativaRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<TreatmentTypeModel>>(entity.Where(x => x.TreatmentClassId == treatmentClassId));
            return _mapper.Map<IEnumerable<TreatmentTypeModel>>(entity.Where(x => x.TreatmentClassId == treatmentClassId && x.Active == active));
        }

       
    }
}
