using AutoMapper;
using Viabilidade.Domain.DTO.Alert;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Interfaces.Notifications;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Repositories.Org;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Models.Alert;
using Viabilidade.Domain.Models.Pagination;
using Viabilidade.Domain.Models.QueryParams.Alert;
using Viabilidade.Domain.Notifications;

namespace Viabilidade.Service.Services.Alert
{
    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _alertaGeradoRepository;
        private readonly IRuleRepository _regraAlertaRepository;
        private readonly IEntityRepository _entityRepository;
        private readonly IMapper _mapper;
        private readonly INotificationHandler<Notification> _notification;
        public AlertService(IAlertRepository alertaGeradoRepository, IRuleRepository regraAlertaRepository, IEntityRepository entityRepository, IMapper mapper, INotificationHandler<Notification> notification)
        {
            _alertaGeradoRepository = alertaGeradoRepository;
            _regraAlertaRepository = regraAlertaRepository;
            _entityRepository = entityRepository;
            _mapper = mapper;
            _notification = notification;
        }
        public async Task<AlertModel> CreateAsync(AlertModel model)
        {
            var entity = await _alertaGeradoRepository.CreateAsync(_mapper.Map<AlertEntity>(model));
            return _mapper.Map<AlertModel>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _alertaGeradoRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<AlertModel>> GetAsync()
        {
            var entity = await _alertaGeradoRepository.GetAsync();
            return _mapper.Map<IEnumerable<AlertModel>>(entity);
        }

        public async Task<AlertModel> GetAsync(int id)
        {
            var entity = await _alertaGeradoRepository.GetAsync(id);
            return _mapper.Map<AlertModel>(entity);
        }

        public async Task<IEnumerable<AlertModel>> GetAsync(IEnumerable<int> ids)
        {
            var entity = await _alertaGeradoRepository.GetAsync(ids);
            return _mapper.Map<IEnumerable<AlertModel>>(entity);
        }

        public async Task<PaginationModel<AlertGroupDto>> GroupAsync(AlertQueryParams queryParams)
        {
            var data = await _alertaGeradoRepository.GroupAsync(queryParams);
            return new PaginationModel<AlertGroupDto>(data.Select(c => c.Item2).FirstOrDefault(), queryParams.Page, queryParams.TotalPage, data.Select(c => c.Item1).ToList());
        }

        public async Task<AlertModel> UpdateAsync(int id, AlertModel model)
        {
            var entity = await _alertaGeradoRepository.UpdateAsync(id, _mapper.Map<AlertEntity>(model));
            return _mapper.Map<AlertModel>(entity);
        }

        public async Task<AlertModel> UpdateUserAsync(int id, AlertModel updateModel)
        {
            var alerta = await _alertaGeradoRepository.GetAsync(id);
            alerta.UserId = updateModel.UserId;
            var entity = await _alertaGeradoRepository.UpdateAsync(id, _mapper.Map<AlertEntity>(alerta));
            return _mapper.Map<AlertModel>(entity);
        }

        public async Task<AlertModel> PreviewAsync(int id)
        {
            var entity = await _alertaGeradoRepository.PreviewAsync(id);
            if(entity == null)
            {
                _notification.AddNotification(404, "Alerta não encontrado");
                return null;
            }
            entity.Entity = await _entityRepository.GetByOriginalEntityAsync((int)entity.EntityId);
            entity.EntityRule.Rule = await _regraAlertaRepository.PreviewAsync((int)entity.EntityRule.RuleId);
            return _mapper.Map<AlertModel>(entity);
        }
        public async Task<IEnumerable<AlertModel>> GetByEntityRuleAsync(int entityRuleId, bool? treated)
        {
            var entity = await _alertaGeradoRepository.GetByEntityRuleAsync(entityRuleId, treated);
            return _mapper.Map<IEnumerable<AlertModel>>(entity);
        }
    }
}
