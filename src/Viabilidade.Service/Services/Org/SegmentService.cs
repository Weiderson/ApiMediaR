using AutoMapper;
using Viabilidade.Domain.Entities.Org;
using Viabilidade.Domain.Interfaces.Cache;
using Viabilidade.Domain.Interfaces.Repositories.Org;
using Viabilidade.Domain.Interfaces.Services.Org;
using Viabilidade.Domain.Models.Org;

namespace Viabilidade.Service.Services.Org
{
    public class SegmentService : ISegmentService
    {
        private readonly ISegmentRepository _segmentoRepository;
        private readonly IStorageCache _cache;
        private readonly IMapper _mapper;
        public SegmentService(ISegmentRepository segmentoRepository, IStorageCache cache, IMapper mapper)
        {
            _segmentoRepository = segmentoRepository;
            _cache = cache;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SegmentModel>> GetAsync(bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("Segment", () => _segmentoRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<SegmentModel>>(entity);
            return _mapper.Map<IEnumerable<SegmentModel>>(entity.Where(x => x.Active == active));
        }

        public async Task<SegmentModel> GetAsync(int id)
        {
            var entity = await _segmentoRepository.GetAsync(id);
            return _mapper.Map<SegmentModel>(entity);
        }
    }
}
