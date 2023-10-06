using AutoMapper;
using Viabilidade.Domain.DTO.Rule;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Interfaces.Notifications;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Models.Alert;
using Viabilidade.Domain.Models.Pagination;
using Viabilidade.Domain.Models.QueryParams.Rule;
using Viabilidade.Domain.Notifications;

namespace Viabilidade.Service.Services.Alert
{
    public class RuleService : IRuleService
    {
        private readonly IRuleRepository _regraAlertaRepository;
        private readonly ITagAlertRepository _alertaTagRepository;
        private readonly IEntityRuleRepository _regraEntidadeRepository;
        private readonly IMapper _mapper;
        private readonly INotificationHandler<Notification> _notification;
        public RuleService(IRuleRepository regraAlertaRepository, ITagAlertRepository alertaTagRepository, IEntityRuleRepository regraEntidadeRepository, IMapper mapper, INotificationHandler<Notification> notification)
        {
            _regraAlertaRepository = regraAlertaRepository;
            _alertaTagRepository = alertaTagRepository;
            _regraEntidadeRepository = regraEntidadeRepository;
            _mapper = mapper;
            _notification = notification;
        }

        public async Task<RuleModel> ActiveInactiveAsync(int id, bool active)
        {
            var regra = await _regraAlertaRepository.GetAsync(id);
            if (regra == null)
            {
                _notification.AddNotification(404, "Regra não encontrada");
                return null;
            }
            regra.Active = active;
            regra.LastUpdateDate = DateTime.Now;
            var entity = await _regraAlertaRepository.UpdateAsync(id, _mapper.Map<RuleEntity>(regra));
            return _mapper.Map<RuleModel>(entity);
        }

        public async Task<RuleModel> CreateAsync(RuleModel model)
        {
            var entity = await _regraAlertaRepository.CreateAsync(_mapper.Map<RuleEntity>(model));
            return _mapper.Map<RuleModel>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _regraAlertaRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<RuleModel>> GetAsync()
        {
            var entity = await _regraAlertaRepository.GetAsync();
            return _mapper.Map<IEnumerable<RuleModel>>(entity);
        }

        public async Task<PaginationModel<RuleGroupDto>> GroupAsync(RuleQueryParams queryParams)
        {
            var data = await ParseData(queryParams);
            foreach (var item in data)
            {
                item.Item1.RuleTags = _mapper.Map<IEnumerable<TagAlertModel>>(await _alertaTagRepository.GetByRuleAsync(item.Item1.Id));
            }
            return new PaginationModel<RuleGroupDto>(data.Select(c => c.Item2).FirstOrDefault(), queryParams.Page, queryParams.TotalPage, data.Select(c => c.Item1).ToList());
        }

        public async Task<RuleModel> GetAsync(int id)
        {
            var entity = await _regraAlertaRepository.GetAsync(id);
            return _mapper.Map<RuleModel>(entity);
        }

        public async Task<RuleModel> PreviewAsync(int id)
        {
            var entity = await _regraAlertaRepository.PreviewAsync(id);
            if (entity == null)
            {
                _notification.AddNotification(404, "Regra não encontrada");
                return null;
            }
            entity.EntityRules = await _regraEntidadeRepository.GetByRuleAsync(entity.Id);
            return _mapper.Map<RuleModel>(entity);
        }

        public async Task<RuleModel> UpdateAsync(int id, RuleModel model)
        {
            var entity = await _regraAlertaRepository.UpdateAsync(id, _mapper.Map<RuleEntity>(model));
            return _mapper.Map<RuleModel>(entity);
        }

        private async Task<IEnumerable<Tuple<RuleGroupDto, int>>> ParseData(RuleQueryParams queryParams)
        {
            switch (queryParams.GroupBy)
            {
                case eRegraAlertaGroupBy.Rule:
                    return await _regraAlertaRepository.GroupByRuleAsync(queryParams);
                case eRegraAlertaGroupBy.Entity:
                    return await _regraAlertaRepository.GroupByEntityAsync(queryParams);
                default:
                    return await _regraAlertaRepository.GroupByRuleAsync(queryParams);

            }
        }

    }
}
