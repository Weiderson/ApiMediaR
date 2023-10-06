using Dapper;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Models.Alert;
using Viabilidade.Infrastructure.Interfaces.DataConnector;

namespace Viabilidade.Infrastructure.Repositories.Alertas
{
    public class IndicatorFilterRepository : BaseRepository<IndicatorFilterEntity>, IIndicatorFilterRepository
    {
        protected override string _database => "Alertas.FiltroIndicador";

        protected override string _selectCollumns => "Id, Descricao as Description, Comando as Command, Ativo as Active";

        public IndicatorFilterRepository(IDbConnector connector): base(connector)
        {
        }

    }
}
