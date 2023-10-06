using AutoMapper;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Interfaces.Cache;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Models.Alert;

namespace Viabilidade.Service.Services.Alert
{
    public class ParameterService : IParameterService
    {
        private readonly IParameterRepository _parametroRepository;
        private readonly IStorageCache _cache;
        private readonly IMapper _mapper;
        public ParameterService(IParameterRepository parametroRepository, IStorageCache cache,IMapper mapper)
        {
            _parametroRepository = parametroRepository;
            _cache = cache;
            _mapper = mapper;
        }
        public async Task<ParameterModel> CreateAsync(ParameterModel model)
        {
            var entity = await _parametroRepository.CreateAsync(_mapper.Map<ParameterEntity>(model));
            return _mapper.Map<ParameterModel>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _parametroRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ParameterModel>> GetAsync(bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("Parameter", () => _parametroRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<ParameterModel>>(entity);
            return _mapper.Map<IEnumerable<ParameterModel>>(entity.Where(x => x.Active == active));
        }

        public async Task<ParameterModel> GetAsync(int id)
        {
            var entity = await _parametroRepository.GetAsync(id);
            return _mapper.Map<ParameterModel>(entity);
        }

        public async Task<ParameterModel> UpdateAsync(int id, ParameterModel model)
        {
            var entity = await _parametroRepository.UpdateAsync(id, _mapper.Map<ParameterEntity>(model));
            return _mapper.Map<ParameterModel>(entity);
        }
    }
}
