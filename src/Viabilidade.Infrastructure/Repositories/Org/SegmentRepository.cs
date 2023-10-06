﻿using Viabilidade.Domain.Entities.Org;
using Viabilidade.Domain.Interfaces.Repositories.Org;
using Viabilidade.Infrastructure.Interfaces.DataConnector;

namespace Viabilidade.Infrastructure.Repositories.Org
{
    public class SegmentRepository : BaseRepository<SegmentEntity>, ISegmentRepository
    {
        protected override string _database => "Org.Segmento";

        protected override string _selectCollumns => "Id, Nome as Name, Ativo as Active";

        public SegmentRepository(IDbConnector connector) : base(connector)
        {
        }

    }
}