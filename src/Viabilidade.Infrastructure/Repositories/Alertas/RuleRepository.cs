﻿using Dapper;
using Microsoft.AspNetCore.Http;
using Viabilidade.Domain.DTO.Rule;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Models.QueryParams.Rule;
using Viabilidade.Infrastructure.ContextAccessor;
using Viabilidade.Infrastructure.Interfaces.DataConnector;

namespace Viabilidade.Infrastructure.Repositories.Alertas
{
    public class RuleRepository : UserContextAccessor, IRuleRepository
    {
        private readonly IDbConnector _connector;
        public RuleRepository(IDbConnector connector, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _connector = connector;
        }
        public async Task<RuleEntity> CreateAsync(RuleEntity entity)
        {
            entity.Id = await _connector.dbConnection.QuerySingleAsync<int>("INSERT INTO Alertas.RegraAlerta" +
                "(Nome, Observacao, TipoId, IndicadorId, OperadorId, FiltroId, ParametroId, Ativo, DataUltimaAlteracao, UsuarioIdCriacao, VersaoMajor, VersaoMinor, VersaoPatch) OUTPUT Inserted.Id " +
                "VALUES" +
                "(@Nome, @Observacao, @TipoId, @IndicadorId, @OperadorId, @FiltroId, @ParametroId, @Ativo, @DataUltimaAlteracao, @UsuarioIdCriacao, @VersaoMajor, @VersaoMinor, @VersaoPatch) "
                , new { Nome = entity.Name, Observacao = entity.Description, TipoId = entity.AlgorithmId, IndicadorId = entity.IndicatorId, OperadorId = entity.OperatorId, FiltroId = entity.IndicatorFilterId, ParametroId = entity.ParameterId, Ativo = entity.Active, DataUltimaAlteracao = entity.LastUpdateDate, UsuarioIdCriacao = _userId, VersaoMajor = entity.VersionMajor, VersaoMinor = entity.VersionMinor, VersaoPatch = entity.VersionPatch }, _connector.dbTransaction);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return Convert.ToBoolean(await _connector.dbConnection.ExecuteAsync("DELETE FROM Alertas.RegraAlerta where Id = @id", new { id }, _connector.dbTransaction));
        }

        public async Task<IEnumerable<RuleEntity>> GetAsync()
        {
            return await _connector.dbConnection.QueryAsync<RuleEntity>("Select Id, Nome as Name, Observacao as Description, TipoId as AlgorithmId, IndicadorId as IndicatorId, OperadorId as OperatorId, FiltroId as IndicatorFilterId, ParametroId as ParameterId, Ativo as Active, DataUltimaAlteracao as LastUpdateDate, UsuarioIdCriacao as UserId, VersaoMajor as VersionMajor, VersaoMinor as VersionMinor, VersaoPatch as VersionPatch from Alertas.RegraAlerta", _connector.dbTransaction);
        }

        public async Task<IEnumerable<Tuple<RuleGroupDto, int>>> GroupByRuleAsync(RuleQueryParams queryParams)
        {
            return await _connector.dbConnection.QueryAsync<RuleGroupDto, int, Tuple<RuleGroupDto, int>>("Select " +
              "ra.Id As Id, " +
              "ra.Nome As RuleName, " +
              "ra.DataUltimaAlteracao as LastChange, " +
              "Count(Distinct re.Id) as EntitiesQuantity, " +
              "Sum(alertas.RegraAlertaEntidadeId) As AlertsQuantity, " +
              "Sum(tratativas.Id) As TreatmentsQuantity," +
              "CASE WHEN Sum(tratativas.Id) = 0 THEN 0 ELSE (convert(float,Sum(tratativas.Id)) / convert(float,Sum(alertas.RegraAlertaEntidadeId)) * 100.0) END AS PercentageTreatment, " +
              "NULL as EntityId, " +
              "NULL as EntityName, " +
              "NULL as RulesQuantity, " +
              "(count (*) over (partition by 1)) AS Total from Alertas.RegraAlerta ra " +
              $"{(queryParams.Pinned ?? false ? " inner join Alertas.AlertaFavorito af on ra.Id = af.AlertaId AND af.UsuarioId = @userId and af.Ativo = 1 " : "")} " +
              $"{(queryParams.Tags != null ? "outer apply(SELECT Top (1) tag.TagId from Alertas.AlertaTag tag where tag.RegraAlertaId = ra.id AND (tag.TagId IN @tags)) as t " : "")} " +
              "left join Alertas.RegraEntidade re on ra.Id = re.AlertaId " +
              "outer apply( " +
                "select Count(ag.RegraAlertaEntidadeId) as RegraAlertaEntidadeId from Alertas.AlertaGerado ag " +
					"inner join Alertas.RegraEntidade regraEntidade on regraEntidade.Id = ag.RegraAlertaEntidadeId " +
				"where ag.RegraAlertaEntidadeId = re.Id " +
			  ") as alertas " +
              "outer apply( " +
                "select Count(alerta_tratativa.Id) as Id from Alertas.AlertaGeradoTratativa alerta_tratativa " +
					"inner join Alertas.R_AlertaGeradoTratativa r_at on r_at.AlertaGeradoTratativaId = alerta_tratativa.Id " +
					"inner join Alertas.AlertaGerado alerta_gerado on alerta_gerado.Id = r_at.AlertaGeradoId " +
					"inner join Alertas.RegraEntidade regra_entidade on regra_entidade.Id = alerta_gerado.RegraAlertaEntidadeId " +
				"where regra_entidade.Id = re.Id " +
			  ") as tratativas " +
              "inner join Org.Entidade e on re.EntidadeId = e.Id " +
              "left join Alertas.R_RegraEntidadeCanal rrc on rrc.RegraEntidadeId = re.Id " +
              "left join Org.Canal c on c.Id = rrc.CanalId " +
              "Where 1 = 1 " +
              $"{(!string.IsNullOrEmpty(queryParams.Search) ? "AND (e.Nome like @search OR ra.Nome like @search) " : "")} " +
              $"{(queryParams.Entities != null ? "AND (e.Id IN @entidades) " : "")} " +
              $"{(queryParams.Tags != null ? "AND (t.TagId IN @tags) " : "")} " +
              $"{(queryParams.Channels != null ? "AND (c.Id IN @canais) " : "")} " +
              $"{(queryParams.InitialDate != null ? " AND ra.DataUltimaAlteracao >= @initialDate " : "")} " +
              $"{(queryParams.FinalDate != null ? " AND ra.DataUltimaAlteracao <= @finalDate " : "")} " +
              $"{(queryParams.Active != null ? " AND ra.Ativo = @ativo " : "")} " +
              "group by ra.Id, ra.Nome, ra.DataUltimaAlteracao " +
              $"ORDER BY {(int)queryParams.OrderCollumn + 1} {queryParams.SortOrders} " +
              "OFFSET @perPage * (@page - 1) ROWS FETCH NEXT @perPage ROWS ONLY",
              map: (regra, tuple) =>
              {
                  return new(regra, tuple);
              },
               param: new
               {
                   perPage = queryParams.TotalPage,
                   entidades = queryParams.Entities,
                   tags = queryParams.Tags,
                   canais = queryParams.Channels,
                   queryParams.Page,
                   ativo = queryParams.Active,
                   search = ("%" + queryParams.Search?.ToLower() + "%"),
                   userId = _userId,
                   initialDate = queryParams.InitialDate,
                   finalDate = queryParams.FinalDate != null ? queryParams.FinalDate.Value.AddDays(1) : queryParams.FinalDate,
               },
              _connector.dbTransaction,
              splitOn: "Total");
        }


        public async Task<IEnumerable<Tuple<RuleGroupDto, int>>> GroupByEntityAsync(RuleQueryParams queryParams)
        {
            return await _connector.dbConnection.QueryAsync<RuleGroupDto, int, Tuple<RuleGroupDto, int>>("Select " +
              "Max(ra.Id) As Id, " +
              "null As RuleName, " +
              "Max(ra.DataUltimaAlteracao) as LastChange, " +
              "NULL AS EntitiesQuantity, " +
              "Sum(alertas.RegraAlertaEntidadeId) As AlertsQuantity, " +
              "Sum(tratativas.Id) As TreatmentsQuantity," +
              "CASE WHEN Sum(tratativas.Id) = 0 THEN 0 ELSE (convert(float,Sum(tratativas.Id)) / convert(float,Sum(alertas.RegraAlertaEntidadeId)) * 100.0) END AS PercentageTreatment, " +
              "e.Id as EntityId, " +
              "e.Nome as EntityName, " +
              "Count(ra.Id) as RulesQuantity, " +
              "(count (*) over (partition by 1)) AS Total from Alertas.RegraAlerta ra " +
              $"{(queryParams.Pinned ?? false ? " inner join Alertas.AlertaFavorito af on ra.Id = af.AlertaId AND af.UsuarioId = @userId and af.Ativo = 1 " : "")} " +
              $"{(queryParams.Tags != null ? "outer apply(SELECT Top (1) tag.TagId from Alertas.AlertaTag tag where tag.RegraAlertaId = ra.id AND (tag.TagId IN @tags)) as t " : "")} " +
              "left join Alertas.RegraEntidade re on ra.Id = re.AlertaId " +
              "outer apply( " +
                "select Count(ag.RegraAlertaEntidadeId) as RegraAlertaEntidadeId from Alertas.AlertaGerado ag " +
                    "inner join Alertas.RegraEntidade regraEntidade on regraEntidade.Id = ag.RegraAlertaEntidadeId " +
                "where ag.RegraAlertaEntidadeId = re.Id " +
              ") as alertas " +
              "outer apply( " +
                "select Count(alerta_tratativa.Id) as Id from Alertas.AlertaGeradoTratativa alerta_tratativa " +
                    "inner join Alertas.R_AlertaGeradoTratativa r_at on r_at.AlertaGeradoTratativaId = alerta_tratativa.Id " +
                    "inner join Alertas.AlertaGerado alerta_gerado on alerta_gerado.Id = r_at.AlertaGeradoId " +
                    "inner join Alertas.RegraEntidade regra_entidade on regra_entidade.Id = alerta_gerado.RegraAlertaEntidadeId " +
                "where regra_entidade.Id = re.Id " +
              ") as tratativas " +
              "inner join Org.Entidade e on re.EntidadeId = e.Id " +
              "left join Alertas.R_RegraEntidadeCanal rrc on rrc.RegraEntidadeId = re.Id " +
              "left join Org.Canal c on c.Id = rrc.CanalId " +
              "Where 1 = 1 " +
              $"{(!string.IsNullOrEmpty(queryParams.Search) ? "AND (e.Nome like @search OR ra.Nome like @search) " : "")} " +
              $"{(queryParams.Entities != null ? "AND (e.Id IN @entidades) " : "")} " +
              $"{(queryParams.Tags != null ? "AND (t.TagId IN @tags) " : "")} " +
              $"{(queryParams.Channels != null ? "AND (c.Id IN @canais) " : "")} " +
              $"{(queryParams.InitialDate != null ? " AND ra.DataUltimaAlteracao >= @initialDate " : "")} " +
              $"{(queryParams.FinalDate != null ? " AND ra.DataUltimaAlteracao <= @finalDate " : "")} " +
              $"{(queryParams.Active != null ? " AND ra.Ativo = @ativo " : "")} " +
              "group by e.Id, e.Nome " +
              $"ORDER BY {(int)queryParams.OrderCollumn + 1} {queryParams.SortOrders} " +
              "OFFSET @perPage * (@page - 1) ROWS FETCH NEXT @perPage ROWS ONLY",
              map: (regra, tuple) =>
              {
                  return new(regra, tuple);
              },
               param: new
               {
                   perPage = queryParams.TotalPage,
                   entidades = queryParams.Entities,
                   tags = queryParams.Tags,
                   canais = queryParams.Channels,
                   queryParams.Page,
                   ativo = queryParams.Active,
                   search = ("%" + queryParams.Search?.ToLower() + "%"),
                   userId = _userId,
                   initialDate = queryParams.InitialDate,
                   finalDate = queryParams.FinalDate != null ? queryParams.FinalDate.Value.AddDays(1) : queryParams.FinalDate,
               },
              _connector.dbTransaction,
              splitOn: "Total");
        }


        public async Task<RuleEntity> GetAsync(int id)
        {
            return await _connector.dbConnection.QueryFirstOrDefaultAsync<RuleEntity>("Select Id, Nome as Name, Observacao as Description, TipoId as AlgorithmId, IndicadorId as IndicatorId, OperadorId as OperatorId, FiltroId as IndicatorFilterId, ParametroId as ParameterId, Ativo as Active, DataUltimaAlteracao as LastUpdateDate, UsuarioIdCriacao as UserId, VersaoMajor as VersionMajor, VersaoMinor as VersionMinor, VersaoPatch as VersionPatch from Alertas.RegraAlerta where id = @id", new { id }, _connector.dbTransaction);
        }
       
        public async Task<RuleEntity> PreviewAsync(int id)
        {
            var listTags = new List<TagAlertEntity>();
            var regras = await _connector.dbConnection.QueryAsync<RuleEntity, AlgorithmEntity, IndicatorEntity, OperatorEntity, ParameterEntity, TagAlertEntity, TagEntity, RuleEntity>(
                "Select ra.Id, ra.Nome as Name, ra.Observacao as Description, ra.TipoId as AlgorithmId, ra.IndicadorId as IndicatorId, ra.OperadorId as OperatorId, ra.FiltroId as IndicatorFilterId, ra.ParametroId as ParameterId, ra.Ativo as Active, ra.DataUltimaAlteracao as LastUpdateDate, ra.UsuarioIdCriacao as UserId, ra.VersaoMajor as VersionMajor, ra.VersaoMinor as VersionMinor, ra.VersaoPatch as VersionPatch, " +
                "alt.Id, alt.Nome as Name, alt.Ativo as Active, " +
                "i.Id, i.Descricao as Description, i.comando as Command, i.Ativo as Active, i.IndicadorSQl as SQLIndicator, i.SegmentoId as SegmentId, " +
                "o.Id, o.Descricao as Description, o.comando as Command, o.Ativo as Active, " +
                "p.Id, p.Ativo as Active, p.CriticidadeBaixa as LowSeverity, p.CriticidadeMedia as MediumSeverity, p.CriticidadeAlta as HighSeverity, p.PeriodoIndicador as EvaluationPeriod, p.PeriodoHistorico as ComparativePeriod, " +
                "atag.Id, atag.RegraAlertaId as RuleId, atag.TagId as TagId, " +
                "ta.Id, ta.Nome as Name, ta.Ativo as Active, ta.IdOriginal as OriginalId " +
                "from Alertas.RegraAlerta ra " +
                "Inner join Alertas.AlgoritmoTipo alt on alt.Id = ra.TipoId " +
                "Inner join Alertas.Indicador i on i.Id = ra.IndicadorId " +
                "Inner join Alertas.Operador o on o.Id = ra.OperadorId " +
                "Left join Alertas.Parametro p on p.Id = ra.ParametroId " +
                "Left join Alertas.AlertaTag atag on atag.RegraAlertaId = ra.Id " +
                "left join Alertas.Tag ta on ta.Id = atag.TagId " +
                "where ra.id = @id",
                map: (rule, algorithm, indicator, operato, parameter, tagAlert, tag) =>
                {
                    if(tagAlert != null)
                    {
                        tagAlert.Tag = tag;
                        listTags.Add(tagAlert);
                    }
                    rule.Algorithm = algorithm;
                    rule.Indicator = indicator;
                    rule.Operator = operato;
                    rule.Parameter = parameter;
                    rule.TagAlerts = listTags;
                    
                    return rule;
                },
                param: new { id },
                _connector.dbTransaction,
                splitOn: "Id, Id, Id, Id, Id");
            return regras.FirstOrDefault();
        }




        public async Task<RuleEntity> UpdateAsync(int id, RuleEntity entity)
        {
            entity.Id = id;
            await _connector.dbConnection.ExecuteAsync("UPDATE Alertas.RegraAlerta " +
               "SET Nome=@Nome, Observacao=@Observacao, TipoId=@TipoId, IndicadorId=@IndicadorId, OperadorId=@OperadorId, FiltroId=@FiltroId, ParametroId=@ParametroId, Ativo=@Ativo, DataUltimaAlteracao=@DataUltimaAlteracao, UsuarioIdCriacao=@UsuarioIdCriacao, VersaoMajor=@VersaoMajor, VersaoMinor=@VersaoMinor, VersaoPatch=@VersaoPatch " +
               "where id = @id", new { id, Nome = entity.Name, Observacao = entity.Description, TipoId = entity.AlgorithmId, IndicadorId = entity.IndicatorId, OperadorId = entity.OperatorId, FiltroId = entity.IndicatorFilterId, ParametroId = entity.ParameterId, Ativo = entity.Active, DataUltimaAlteracao = entity.LastUpdateDate, UsuarioIdCriacao = _userId, VersaoMajor = entity.VersionMajor, VersaoMinor = entity.VersionMinor, VersaoPatch = entity.VersionPatch }, _connector.dbTransaction);
            return entity;
        }
    }
}
