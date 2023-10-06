using AutoMapper;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Models.Alert;

namespace Viabilidade.Service.Services.Alert
{
    public class RChannelEntityRuleService : IRChannelEntityRuleService
    {
        private readonly IRChannelEntityRuleRepository _rRegraEntidadeCanalRepository;
        private readonly IMapper _mapper;
        public RChannelEntityRuleService(IRChannelEntityRuleRepository rRegraEntidadeCanalRepository, IMapper mapper)
        {
            _rRegraEntidadeCanalRepository = rRegraEntidadeCanalRepository;
            _mapper = mapper;
        }
        public async Task<RChannelEntityRuleModel> CreateAsync(RChannelEntityRuleModel model)
        {
            var entity = await _rRegraEntidadeCanalRepository.CreateAsync(_mapper.Map<RChannelEntityRuleEntity>(model));
            return _mapper.Map<RChannelEntityRuleModel>(entity);
        }

        public async Task<bool> DeleteByEntityRuleAsync(IEnumerable<int> entityRuleIds)
        {
            return await _rRegraEntidadeCanalRepository.DeleteByEntityRuleAsync(entityRuleIds);
        }
    }
}
