using AutoMapper;
using System.Data;
using Viabilidade.Domain.DTO.EntityRule;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Interfaces.Services.Host;
using Viabilidade.Domain.Models.Alert;
using Viabilidade.Domain.Models.Pagination;
using Viabilidade.Domain.Models.QueryParams.EntityRule;

namespace Viabilidade.Service.Services.Alert
{
    public class EntityRuleService : IEntityRuleService
    {
        private readonly IEntityRuleRepository _regraEntidadeRepository;
        private readonly IUserService _userService;
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IAlertRepository _alertRepository;
        private readonly IMapper _mapper;
        public EntityRuleService(IEntityRuleRepository regraEntidadeRepository, IUserService userService, ITreatmentRepository treatmentRepository, IAlertRepository alertRepository, IMapper mapper)
        {
            _regraEntidadeRepository = regraEntidadeRepository;
            _userService = userService;
            _treatmentRepository = treatmentRepository;
            _alertRepository = alertRepository;
            _mapper = mapper;
        }
        public async Task<EntityRuleModel> CreateAsync(EntityRuleModel model)
        {
            var entity = await _regraEntidadeRepository.CreateAsync(_mapper.Map<EntityRuleEntity>(model));
            return _mapper.Map<EntityRuleModel>(entity);
        }

        public async Task<EntityRuleModel> GetAsync(int id)
        {
            var entity = await _regraEntidadeRepository.GetAsync(id);
            return _mapper.Map<EntityRuleModel>(entity);
        }

        public async Task<EntityRuleModel> PreviewAsync(int id)
        {
            var entity = await _regraEntidadeRepository.PreviewAsync(id);
            return _mapper.Map<EntityRuleModel>(entity);
        }

        public async Task<EntityRuleModel> PreviewAsync(int regraId, int entidadeId)
        {
            var entity = await _regraEntidadeRepository.PreviewAsync(regraId, entidadeId);
            return _mapper.Map<EntityRuleModel>(entity);
        }

        public async Task<EntityRuleModel> UpdateAsync(int id, EntityRuleModel model)
        {
            var entity = await _regraEntidadeRepository.UpdateAsync(id, _mapper.Map<EntityRuleEntity>(model));
            return _mapper.Map<EntityRuleModel>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _regraEntidadeRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteByRuleAsync(int ruleId)
        {
            return await _regraEntidadeRepository.DeleteByRuleAsync(ruleId);
        }

        public async Task<IEnumerable<EntityRuleModel>> GetByRuleAsync(int ruleId)
        {
            var models = _mapper.Map<IEnumerable<EntityRuleModel>>(await _regraEntidadeRepository.GetByRuleAsync(ruleId));
            foreach (var entityRule in models)
                entityRule.UserName = await _userService.GetUserNameAsync((Guid)entityRule.UserId);
            return models;
        }

        public async Task<IEnumerable<EntityRuleModel>> GetByEntityAsync(int entityId)
        {
            var models = _mapper.Map<IEnumerable<EntityRuleModel>>(await _regraEntidadeRepository.GetByEntityAsync(entityId));
            foreach (var entityRule in models)
                entityRule.UserName = await _userService.GetUserNameAsync((Guid)entityRule.UserId);
            return models;
        }

        public async Task<PaginationModel<EntityRuleGroupByRuleDto>> GroupByRuleAsync(int ruleId, EntityRuleQueryParams queryParams)
        {
            var data = await _regraEntidadeRepository.GroupByRuleAsync(ruleId, queryParams);
            if (data.Any())
                await Parallel.ForEachAsync(data, new ParallelOptions() { MaxDegreeOfParallelism = data.Count() }, async (entityRule, ct) =>
                {
                    entityRule.Item1.UserName = await _userService.GetUserNameAsync(entityRule.Item1.UserId);
                    //Calculo de Aderencia
                    /*var ruleApplicant = await RuleEntityApplicant(entityRule.Item1.Id, entityRule.Item1.AlertsQuantity, entityRule.Item1.TreatmentsQuantity, entityRule.Item1.PercentageTreatment, entityRule.Item1.RuleLastChange);
                    entityRule.Item1.RuleApplicant = ruleApplicant.Item1;
                    entityRule.Item1.RuleApplicantDescription = ruleApplicant.Item2;*/
                });
            return new PaginationModel<EntityRuleGroupByRuleDto>(data.Select(c => c.Item2).FirstOrDefault(), queryParams.Page, queryParams.TotalPage, data.Select(c => c.Item1).ToList());
        }

        public async Task<PaginationModel<EntityRuleGroupByEntityDto>> GroupByEntityAsync(int entityId, EntityRuleQueryParams queryParams)
        {
            var data = await _regraEntidadeRepository.GroupByEntityAsync(entityId, queryParams);

            if (data.Any())
                await Parallel.ForEachAsync(data, new ParallelOptions() { MaxDegreeOfParallelism = data.Count() }, async (entityRule, ct) =>
                {
                    entityRule.Item1.UserName = await _userService.GetUserNameAsync(entityRule.Item1.UserId);
                    //Calculo de Aderencia
                    /*var ruleApplicant = await RuleEntityApplicant(entityRule.Item1.Id, entityRule.Item1.AlertsQuantity, entityRule.Item1.TreatmentsQuantity, entityRule.Item1.PercentageTreatment, entityRule.Item1.RuleLastChange);
                    entityRule.Item1.RuleApplicant = ruleApplicant.Item1;
                    entityRule.Item1.RuleApplicantDescription = ruleApplicant.Item2;*/
                });

            return new PaginationModel<EntityRuleGroupByEntityDto>(data.Select(c => c.Item2).FirstOrDefault(), queryParams.Page, queryParams.TotalPage, data.Select(c => c.Item1).ToList());
        }

        private async Task<(bool, string)> RuleEntityApplicant(int entityRuleId, int alertsQuantity, int treatmentsQuantity, decimal percentageTreatment, DateTime lastChange)
        {

            if (await AderenciaPorTratativa(entityRuleId, treatmentsQuantity, percentageTreatment))
                return (true, "Aderência por poucas tratativas (entre 0% e 50%) ou problemas maior que 50%");

            if (await AderenciaPorAlerta(entityRuleId, lastChange))
                return (true, "Aderência por 30 dias (ou mais) sem disparo");

            if (await AderenciaPorCriticidade(entityRuleId, alertsQuantity, percentageTreatment))
                return (true, "Alertas com criticidade baixa (maior que 40%) e poucas tratativas (menor que 60%)");

            return (false, null);

        }

        private async Task<bool> AderenciaPorTratativa(int entityRuleId, int treatmentsQuantity, decimal percentageTreatment)
        {
            if (percentageTreatment <= 50)
            {
                var totalProblems = await _treatmentRepository.CountByEntityRuleWasProblemGroupAsync(entityRuleId);
                return ((double)totalProblems / treatmentsQuantity) > 0.5;
            }
            return false;
        }
        private async Task<bool> AderenciaPorAlerta(int entityRuleId, DateTime lastChange)
        {
            var dataLastAlert = (await _alertRepository.GetLastByEntityRuleAsync(entityRuleId))?.Version ?? lastChange;
            if ((int)dataLastAlert.Subtract(DateTime.Now).TotalDays > 30)
                return true;

            return false;
        }

        private async Task<bool> AderenciaPorCriticidade(int entityRuleId, int alertsQuantity, decimal percentageTreatment)
        {
            var totalLowSeverity = await _alertRepository.CountLowSeverityByEntityRuleAsync(entityRuleId);
            var severityPercente = (double)totalLowSeverity / alertsQuantity;
            if (severityPercente > 0.4 && percentageTreatment < 60)
                return true;

            return false;
        }
    }
}
