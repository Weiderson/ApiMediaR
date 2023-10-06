using AutoMapper;
using System;
using Viabilidade.Domain.Interfaces.Cache;
using Viabilidade.Domain.Interfaces.Repositories.Org;
using Viabilidade.Domain.Interfaces.Services.Org;
using Viabilidade.Domain.Models.Org;

namespace Viabilidade.Service.Services.Org
{
    public class SquadService : ISquadService
    {
        private readonly ISquadRepository _squadRepository;
        private readonly IStorageCache _cache;
        private readonly IMapper _mapper;
        public SquadService(ISquadRepository squadRepository, IStorageCache cache, IMapper mapper)
        {
            _squadRepository = squadRepository;
            _cache = cache;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SquadModel>> GetAsync(bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("Squad", () => _squadRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<SquadModel>>(entity);
            return _mapper.Map<IEnumerable<SquadModel>>(entity.Where(x => x.Active == active));
        }

        public async Task<SquadModel> GetAsync(int id)
        {
            var entity = await _squadRepository.GetAsync(id);
            return _mapper.Map<SquadModel>(entity);
        }
    }
}
