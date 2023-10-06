using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Infrastructure.Interfaces.DataConnector;

namespace Viabilidade.Infrastructure.Repositories.Alertas
{
    public class AlgorithmRepository : BaseRepository<AlgorithmEntity>, IAlgorithmRepository
    {
        protected override string _database => "Alertas.AlgoritmoTipo";

        protected override string _selectCollumns => "Id, Nome as Name, Ativo as Active";

        public AlgorithmRepository(IDbConnector connector) : base(connector)
        {
        }

    }
}
