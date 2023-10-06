using AutoMapper;
using Viabilidade.Domain.Interfaces.Cache;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Models.Alert;

namespace Viabilidade.Service.Services.Alert
{
    public class OperatorService : IOperatorService
    {
        private readonly IOperatorRepository _operadorRepository;
        private readonly IStorageCache _cache;
        private readonly IMapper _mapper;
        public OperatorService(IOperatorRepository operadorRepository, IStorageCache cache,IMapper mapper)
        {
            _operadorRepository = operadorRepository;
            _cache = cache;
            _mapper = mapper;
        }
        public async Task<IEnumerable<OperatorModel>> GetAsync(bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("Operator", () => _operadorRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<OperatorModel>>(entity);
            return _mapper.Map<IEnumerable<OperatorModel>>(entity.Where(x => x.Active == active));
        }

        public async Task<OperatorModel> GetAsync(int id)
        {
            var entity = await _operadorRepository.GetAsync(id);
            return _mapper.Map<OperatorModel>(entity);
        }
    }
}
