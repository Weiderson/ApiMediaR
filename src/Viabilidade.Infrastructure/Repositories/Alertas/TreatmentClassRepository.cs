using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Infrastructure.Interfaces.DataConnector;

namespace Viabilidade.Infrastructure.Repositories.Alertas
{
    public class TreatmentClassRepository : BaseRepository<TreatmentClassEntity>, ITreatmentClassRepository
    {
        protected override string _database => "Alertas.ClasseTratativa";

        protected override string _selectCollumns => "Id, Nome as Name, Conceito as Concept, Ativo as Active";

        public TreatmentClassRepository(IDbConnector connector) : base(connector)
        {
        }

    }
}
