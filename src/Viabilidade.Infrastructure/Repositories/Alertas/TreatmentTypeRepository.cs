using Dapper;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Infrastructure.Interfaces.DataConnector;

namespace Viabilidade.Infrastructure.Repositories.Alertas
{
    public class TreatmentTypeRepository : BaseRepository<TreatmentTypeEntity>, ITreatmentTypeRepository
    {
        private readonly IDbConnector _connector;
        protected override string _database => "Alertas.TipoTratativa";

        protected override string _selectCollumns => "Id, Descricao as Description, ClasseTratativaId as TreatmentClassId, TipoTratativaConceito as TreatmentTypeConcept, Ativo as Active";

        public TreatmentTypeRepository(IDbConnector connector): base(connector)
        {
            _connector = connector;
        }

        public async Task<IEnumerable<TreatmentTypeEntity>> GetByTreatmentClassAsync(int treatmentClassId, bool? active = null)
        {
            return await _connector.dbConnection.QueryAsync<TreatmentTypeEntity>($"Select {_selectCollumns} from {_database} where classeTratativaId = @treatmentClassId {(active != null ? "and ativo = @active" : "")}", new { treatmentClassId, active }, _connector.dbTransaction);
        }
    }
}
