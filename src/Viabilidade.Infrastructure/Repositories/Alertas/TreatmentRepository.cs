﻿using Dapper;
using Microsoft.AspNetCore.Http;
using Viabilidade.Domain.DTO.Treatment;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Entities.Org;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Models.QueryParams.Treatment;
using Viabilidade.Infrastructure.ContextAccessor;
using Viabilidade.Infrastructure.Interfaces.DataConnector;

namespace Viabilidade.Infrastructure.Repositories.Alertas
{
    public class TreatmentRepository : UserContextAccessor, ITreatmentRepository
    {
        private readonly IDbConnector _connector;
        public TreatmentRepository(IDbConnector connector, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _connector = connector;
        }
        public async Task<TreatmentEntity> CreateAsync(TreatmentEntity entity)
        {
            entity.UserId = new Guid(_userId);
            entity.Id = await _connector.dbConnection.QuerySingleAsync<int>("INSERT INTO Alertas.AlertaGeradoTratativa" +
                "(Data, UsuarioIdResponsavel, ClasseTratativaId, AlertaTratativaTipoId, Observacao, Ativo) OUTPUT Inserted.Id " +
                "VALUES" +
                "(@Data, @UsuarioIdResponsavel, @ClasseTratativaId, @AlertaTratativaTipoId, @Observacao, @Ativo)"
                , new { Data = entity.Date, UsuarioIdResponsavel = _userId, ClasseTratativaId = entity.TreatmentClassId, AlertaTratativaTipoId = entity.TreatmentTypeId, Observacao = entity.Description, Ativo = entity.Active }, _connector.dbTransaction);
            return entity;
        }

        public async Task<IEnumerable<Tuple<TreatmentGroupDto, int>>> GroupAsync(TreatmentQueryParams queryParams)
        {

            return await _connector.dbConnection.QueryAsync<TreatmentGroupDto, int, Tuple<TreatmentGroupDto, int>>("SELECT " +
              "MAX(agt.Id) AS Id, " +
              "e.Nome EntityName, " +
              "agc.CanalNome as ChannelName, " +
              "s.Nome AS SquadName, " +
              "ra.Nome AS AlertName, " +
              "COUNT(DISTINCT (agt.Id)) AS TreatmentsQuantity, " +
              "MAX(ct.Nome) AS LastClassName, " +
              "(CAST(COUNT(DISTINCT (CASE WHEN agt.ClasseTratativaId = 2 THEN agt.Data END)) AS decimal(18,2)) / CAST(COUNT(DISTINCT (agt.Data)) AS decimal(18,2))) * 100.0 AS PercentageProblem, " +
              "MAX(agt.Data) AS Date, " +
              "MAX(ragt.AlertaGeradoId) AS AlertId, " +
              "MAX(ag.RegraAlertaEntidadeId) AS EntityRuleId, " +
              "(count (*) over (partition by 1)) AS Total " +
              "FROM Alertas.AlertaGeradoTratativa AS agt " +
              "INNER JOIN Alertas.R_AlertaGeradoTratativa AS ragt ON agt.Id = ragt.AlertaGeradoTratativaId " +
              "INNER JOIN Alertas.AlertaGerado AS ag ON ragt.AlertaGeradoId = ag.Id " +
              "LEFT JOIN Alertas.AlertaGeradoCanal AS agc ON ag.Id = agc.AlertaGeradoId " +
              "LEFT JOIN Alertas.RegraEntidade AS re ON ag.RegraAlertaEntidadeId = re.Id " +
              "LEFT JOIN Alertas.RegraAlerta AS ra ON re.AlertaId = ra.Id " +
              $"{(queryParams.Tags != null ? "outer apply(SELECT Top (1) tag.TagId from Alertas.AlertaTag tag where tag.RegraAlertaId = ra.id AND (tag.TagId IN @tags)) as t " : "")} " +
              "INNER JOIN Org.Entidade AS e ON re.EntidadeId = e.Id " +
              "LEFT JOIN Org.R_SquadEntidade AS rse ON e.Id = rse.EntidadeId " +
              "LEFT JOIN Org.Squad AS s ON rse.SquadId = s.Id " +
              "LEFT JOIN Alertas.ClasseTratativa AS ct ON agt.ClasseTratativaId = ct.Id " +
              "where 1 = 1 " +
               $"{(!string.IsNullOrEmpty(queryParams.Search) ? "AND (ag.EntidadeNome like @search OR e.Nome like @search OR ra.Nome like @search OR agc.CanalNome like @search) " : "")} " +
               $"{(queryParams.Squads != null ? "AND (s.Id IN @squads) " : "")} " +
               $"{(queryParams.Entities != null ? "AND (e.Id IN @entidades OR ag.EntidadeId IN @entidades)  " : "")} " +
               $"{(queryParams.Responsibles != null ? "AND (ag.UsuarioIdResponsavel IN @responsaveis) " : "")} " +
               $"{(queryParams.Tags != null ? "AND (t.TagId IN @tags) " : "")} " +
               $"{(queryParams.Channels != null ? "AND (agc.CanalId IN @canais) " : "")} " +
               $"{(queryParams.InitialDate != null ? " AND agt.Data >= @initialDate " : "")} " +
               $"{(queryParams.FinalDate != null ? " AND agt.Data <= @finalDate " : "")} " +
              "GROUP BY ag.RegraAlertaEntidadeId, ra.Nome, e.Id, e.Nome, agc.CanalId, rse.SquadId, s.Nome, agc.CanalNome " +
              $"ORDER BY {(int)queryParams.OrderCollumn + 1} {queryParams.SortOrders} " +
              "OFFSET @perPage * (@page - 1) ROWS FETCH NEXT @perPage ROWS ONLY",
              map: (tratativa, tuple) =>
              {
                  return new(tratativa, tuple);
              },
              param: new
              {
                  perPage = queryParams.TotalPage,
                  squads = queryParams.Squads,
                  entidades = queryParams.Entities,
                  responsaveis = queryParams.Responsibles,
                  tags = queryParams.Tags,
                  canais = queryParams.Channels,
                  queryParams.Page,
                  search = ("%" + queryParams.Search?.ToLower() + "%"),
                  userId = _userId,
                  initialDate = queryParams.InitialDate,
                  finalDate = queryParams.FinalDate != null ? queryParams.FinalDate.Value.AddDays(1) : queryParams.FinalDate,
              },
              _connector.dbTransaction,
              splitOn: "Total");
        }

        public async Task<TreatmentEntity> GetAsync(int id)
        {
            return await _connector.dbConnection.QueryFirstOrDefaultAsync<TreatmentEntity>("Select Id, Data as Date, UsuarioIdResponsavel as UserId, ClasseTratativaId as TreatmentClassId, AlertaTratativaTipoId as TreatmentTypeId, Observacao as Description, Ativo as Active from Alertas.AlertaGeradoTratativa where id = @id", new { id }, _connector.dbTransaction);
        }

        public async Task<TreatmentEntity> PreviewAsync(int id)
        {
            var listAnexos = new List<AttachmentEntity>();
            return (await _connector.dbConnection.QueryAsync<TreatmentEntity, TreatmentClassEntity, TreatmentTypeEntity, AttachmentEntity, TreatmentEntity>(
                "Select agt.Id, agt.Data as Date, agt.UsuarioIdResponsavel as UserId, agt.ClasseTratativaId as TreatmentClassId, agt.AlertaTratativaTipoId as TreatmentTypeId, agt.Observacao as Description, agt.Ativo as Active, " +
                "ct.Id, ct.Nome as Name, ct.Conceito as Concept, ct.Ativo as Active, " +
                "tt.Id, tt.Descricao as Description, tt.ClasseTratativaId as TreatmentClassId, tt.TipoTratativaConceito as TreatmentTypeConcept, tt.Ativo as Active, " +
                "ant.Id, ant.TratativaId as TreatmentId, ant.CaminhoArquivo as PathFile, ant.DataUpload as UploadDate, ant.UsuarioIdUpload as UserId, ant.Ativo as Active, ant.NomeArquivo as FileName " +
                "from Alertas.AlertaGeradoTratativa agt " +
                "INNER JOIN Alertas.ClasseTratativa ct on ct.Id = agt.ClasseTratativaId " +
                "INNER JOIN Alertas.TipoTratativa tt on tt.Id = agt.AlertaTratativaTipoId " +
                "LEFT JOIN Alertas.AnexoTratativa ant on ant.TratativaId = agt.Id " +
                "where agt.Id = @id",
                map: (treatment, treatmentClass, treatmentType, attachment) =>
                {
                    if (attachment != null)
                    {
                        if (listAnexos.FirstOrDefault(x => x.Id == attachment.Id) == null)
                            listAnexos.Add(attachment);
                    }
                    treatment.TreatmentClass = treatmentClass;
                    treatment.TreatmentType = treatmentType;
                    treatment.Attachments = listAnexos;
                    return treatment;
                },
                param: new { id },
                _connector.dbTransaction,
                splitOn: "Id, Id, Id, Id, Id")).FirstOrDefault();
        }


        public async Task<IEnumerable<TreatmentByEntityRuleGroupDto>> GetByEntityRuleGroupAsync(int entityRuleId)
        {
            return await _connector.dbConnection.QueryAsync<TreatmentByEntityRuleGroupDto>("Select agt.Id, ct.Id as TreatmentClassId, ct.Nome as TreatmentClass, agt.Observacao as Description, count(agt.Id) as AlertsQuantity, agt.UsuarioIdResponsavel as UserId, agt.Data as Date, Max(ag.Id) as AlertId from Alertas.AlertaGeradoTratativa AS agt " +
                "INNER JOIN Alertas.R_AlertaGeradoTratativa AS ragt ON agt.Id = ragt.AlertaGeradoTratativaId " +
                "INNER JOIN Alertas.AlertaGerado AS ag ON ragt.AlertaGeradoId = ag.Id " +
                "INNER JOIN Alertas.ClasseTratativa AS ct ON ct.Id = agt.ClasseTratativaId " +
                "where ag.RegraAlertaEntidadeId = @entityRuleId " +
                "group by agt.id, ct.Id, ct.Nome, agt.Observacao, agt.UsuarioIdResponsavel, agt.Data order by agt.Id desc"
                , new { entityRuleId }, _connector.dbTransaction);

        }

        public async Task<int> CountByEntityRuleWasProblemGroupAsync(int entityRuleId)
        {
            return await _connector.dbConnection.ExecuteScalarAsync<int>("Select Count(agt.Id) from Alertas.AlertaGeradoTratativa AS agt " +
                "INNER JOIN Alertas.R_AlertaGeradoTratativa AS ragt ON agt.Id = ragt.AlertaGeradoTratativaId " +
                "INNER JOIN Alertas.AlertaGerado AS ag ON ragt.AlertaGeradoId = ag.Id " +
                "INNER JOIN Alertas.ClasseTratativa AS ct ON ct.Id = agt.ClasseTratativaId " +
                "where ag.RegraAlertaEntidadeId = @entityRuleId and agt.ClasseTratativaId = 2 " +
                "group by agt.id, ct.Id, ct.Nome, agt.Observacao, agt.UsuarioIdResponsavel, agt.Data"
                , new { entityRuleId }, _connector.dbTransaction);

        }

        public async Task<TreatmentEntity> PreviewDetailAsync(int id)
        {
            var listAnexos = new List<AttachmentEntity>();
            var listAlertas = new List<AlertEntity>();
            return (await _connector.dbConnection.QueryAsync<TreatmentEntity, AlertEntity, EntityRuleEntity, EntityEntity, TreatmentClassEntity, TreatmentTypeEntity, AttachmentEntity, TreatmentEntity>(
                "Select agt.Id, agt.Data as Date, agt.UsuarioIdResponsavel as UserId, agt.ClasseTratativaId as TreatmentClassId, agt.AlertaTratativaTipoId as TreatmentTypeId, agt.Observacao as Description, agt.Ativo as Active, " +
                "ag.Id, ag.EntidadeId as EntityId, ag.EntidadeNome as EntityName, ag.RegraAlertaEntidadeId as EntityRuleId, ag.RegraAlertanome as RuleName, ag.Versao as Version, ag.DiaMinIndicador as IndicatorFirstDate, ag.DiaMaxIndicador as IndicadorLastDate, ag.Criticidade as Severity, ag.Indicador as Indicator, ag.IndicadorReferenciabaixo as LowReferenceIndicator, ag.IndicadorReferenciamedio as MediumReferenceIndicator, ag.IndicadorReferenciaalto as HighReferenceIndicator, ag.AlertaStatusId as StatusId, ag.DataFinalizacao as FinishDate, ag.UsuarioIdResponsavel as UserId, ag.Ativo as Active, ag.Tratado as Treated, ag.ValorIndicador as IndicatorValue, ag.PorcentagemIndicador as PercentageIndicator, " +
                "re.Id, re.AlertaId as RuleId, re.EntidadeId as EntityId, re.NmSubEntidade as SubEntityNumber, re.UsuarioIdResponsavel as UserId, re.FiltroId as IndicatorFilterId, re.ParametroId as ParameterId, re.Ativo as Active, " +
                "e.Id, e.Nome as Name, e.EntidadeIdOriginal as OriginalEntityId, e.Ativo as Active, e.SegmentoId as SegmentId, " +
                "ct.Id, ct.Nome as Name, ct.Conceito as Concept, ct.Ativo as Active, " +
                "tt.Id, tt.Descricao as Description, tt.ClasseTratativaId as TreatmentClassId, tt.TipoTratativaConceito as TreatmentTypeConcept, tt.Ativo as Active, " +
                "ant.Id, ant.TratativaId as TreatmentId, ant.CaminhoArquivo as PathFile, ant.DataUpload as UploadDate, ant.UsuarioIdUpload as UserId, ant.Ativo as Active, ant.NomeArquivo as FileName " +
                "from Alertas.AlertaGeradoTratativa agt " +
                "INNER JOIN Alertas.R_AlertaGeradoTratativa ragt on ragt.AlertaGeradoTratativaId = agt.Id " +
                "INNER JOIN Alertas.AlertaGerado ag on ag.Id = ragt.AlertaGeradoId " +
                "INNER JOIN Alertas.RegraEntidade re on re.Id = ag.RegraAlertaEntidadeId " +
                "INNER JOIN Org.Entidade e on e.Id = re.EntidadeId " +
                "INNER JOIN Alertas.RegraAlerta ra on ra.Id = re.AlertaId " +
                "INNER JOIN Alertas.ClasseTratativa ct on ct.Id = agt.ClasseTratativaId " +
                "INNER JOIN Alertas.TipoTratativa tt on tt.Id = agt.AlertaTratativaTipoId " +
                "LEFT JOIN Alertas.AnexoTratativa ant on ant.TratativaId = agt.Id " +
                "where agt.Id = @id",
                map: (treatment, alerts, entityRule, entity, treatmentClass, treatmentType, attachment) =>
                {
                    if (alerts != null)
                    {
                        alerts.EntityRule = entityRule;
                        if (listAlertas.FirstOrDefault(x => x.Id == alerts.Id) == null)
                            listAlertas.Add(alerts);
                    }
                    if (attachment != null)
                    {
                        if (listAnexos.FirstOrDefault(x => x.Id == attachment.Id) == null)
                            listAnexos.Add(attachment);
                    }
                    treatment.TreatmentClass = treatmentClass;
                    treatment.TreatmentType = treatmentType;
                    treatment.Attachments = listAnexos;
                    treatment.Alerts = listAlertas;
                    return treatment;
                },
                param: new { id },
                _connector.dbTransaction,
                splitOn: "Id, Id, Id, Id, Id, Id, Id")).FirstOrDefault();
        }

    }
}
