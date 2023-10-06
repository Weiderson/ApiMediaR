using AutoMapper;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Exceptions;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Models.Alert;

namespace Viabilidade.Service.Services.Alert
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _anexoTratativaRepository;
        private readonly IMapper _mapper;
        public AttachmentService(IAttachmentRepository anexoTratativaRepository, IMapper mapper)
        {
            _anexoTratativaRepository = anexoTratativaRepository;
            _mapper = mapper;
        }
        public async Task<AttachmentModel> CreateAsync(AttachmentModel model)
        {
            var entity = await _anexoTratativaRepository.CreateAsync(_mapper.Map<AttachmentEntity>(model));
            return _mapper.Map<AttachmentModel>(entity);
        }
      
        public async Task<AttachmentModel> GetAsync(int id)
        {
            var entity = await _anexoTratativaRepository.GetAsync(id);
            if(entity == null)
                throw new DomainException("Arquivo não encontrado", 404);
            return _mapper.Map<AttachmentModel>(entity);
        }

        public async Task<IEnumerable<AttachmentModel>> GetByTreatmentAsync(int treatmentId)
        {
            var entity = await _anexoTratativaRepository.GetByTreatmentAsync(treatmentId);
            return _mapper.Map<IEnumerable<AttachmentModel>>(entity);
        }
    }
}
