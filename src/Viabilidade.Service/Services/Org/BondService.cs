using AutoMapper;
using Viabilidade.Domain.Interfaces.Repositories.Org.Views;
using Viabilidade.Domain.Interfaces.Services.Org;
using Viabilidade.Domain.Models.Views;

namespace Viabilidade.Service.Services.Org
{
    public class BondService : IBondService
    {
        private readonly IVwChannelSquadEntityRepository _vwCanalSquadEntidadeRepository;
        private readonly IMapper _mapper;
        public BondService(IVwChannelSquadEntityRepository vwCanalSquadEntidadeRepository, IMapper mapper)
        {
            _vwCanalSquadEntidadeRepository = vwCanalSquadEntidadeRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BondModel>> GetAsync(string search, int segmentId)
        {
           var view = await _vwCanalSquadEntidadeRepository.GetAsync(search, segmentId);
            return _mapper.Map<IEnumerable<BondModel>>(view);
        }
    }
}
