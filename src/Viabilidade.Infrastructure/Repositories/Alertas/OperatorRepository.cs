using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Infrastructure.Interfaces.DataConnector;

namespace Viabilidade.Infrastructure.Repositories.Alertas
{
    public class OperatorRepository : BaseRepository<OperatorEntity>, IOperatorRepository
    {
        protected override string _database => "Alertas.Operador";

        protected override string _selectCollumns => "Id, Descricao as Description, Comando as Command, Ativo as Active";

        public OperatorRepository(IDbConnector connector) : base(connector)
        {
        }

    }
}
