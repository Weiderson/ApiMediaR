using AutoMapper;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Interfaces.Services.Host;
using Viabilidade.Domain.Models.Alert;

namespace Viabilidade.Service.Services.Alert
{
    public class SilencedAlertService : ISilencedAlertService
    {
        private readonly ISilencedAlertRepository _alertaGeradoSilenciadoRepository;
        private readonly IMapper _mapper;
        public SilencedAlertService(ISilencedAlertRepository alertaGeradoSilenciadoRepository, IMapper mapper)
        {
            _alertaGeradoSilenciadoRepository = alertaGeradoSilenciadoRepository;
            _mapper = mapper;
        }
        public async Task<SilencedAlertModel> CreateAsync(SilencedAlertModel model)
        {
            var entity = await _alertaGeradoSilenciadoRepository.CreateAsync(_mapper.Map<SilencedAlertEntity>(model));
            return _mapper.Map<SilencedAlertModel>(entity);
        }

    }
}
