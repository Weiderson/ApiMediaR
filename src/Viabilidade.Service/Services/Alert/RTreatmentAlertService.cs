using AutoMapper;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Models.Alert;

namespace Viabilidade.Service.Services.Alert
{
    public class RTreatmentAlertService : IRTreatmentAlertService
    {
        private readonly IRTreatmentAlertRepository _rAlertaGeradoTratativaRepository;
        private readonly IMapper _mapper;
        public RTreatmentAlertService(IRTreatmentAlertRepository rAlertaGeradoTratativaRepository, IMapper mapper)
        {
            _rAlertaGeradoTratativaRepository = rAlertaGeradoTratativaRepository;
            _mapper = mapper;
        }
        public async Task<RTreatmentAlertModel> CreateAsync(RTreatmentAlertModel model)
        {
            var entity = await _rAlertaGeradoTratativaRepository.CreateAsync(_mapper.Map<RTreatmentAlertEntity>(model));
            return _mapper.Map<RTreatmentAlertModel>(entity);
        }
    }
}
