using Dapper;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Infrastructure.Interfaces.DataConnector;

namespace Viabilidade.Infrastructure.Repositories.Alertas
{
    public class IndicatorRepository : BaseRepository<IndicatorEntity>, IIndicatorRepository
    {
        private readonly IDbConnector _connector;
        protected override string _database => "Alertas.Indicador";

        protected override string _selectCollumns => "Id, Descricao as Description, Comando as Command, Ativo as Active, IndicadorSQL as SQLIndicator, SegmentoId as SegmentId";

        public IndicatorRepository(IDbConnector connector): base(connector)
        {
            _connector = connector;
        }

        public async Task<IEnumerable<IndicatorEntity>> GetBySegmentAsync(int segmentId, bool? active = null)
        {
            return await _connector.dbConnection.QueryAsync<IndicatorEntity>($"Select {_selectCollumns} from {_database} where segmentoId = @segmentId {(active != null ? "and ativo = @active" : "")}", new { segmentId, active }, _connector.dbTransaction);
        }
    }
}
