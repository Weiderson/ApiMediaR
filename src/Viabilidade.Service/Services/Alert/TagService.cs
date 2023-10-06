using AutoMapper;
using System;
using Viabilidade.Domain.Interfaces.Cache;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Models.Alert;

namespace Viabilidade.Service.Services.Alert
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IStorageCache _cache;
        private readonly IMapper _mapper;
        public TagService(ITagRepository tagRepository, IStorageCache cache,IMapper mapper)
        {
            _tagRepository = tagRepository;
            _cache = cache;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TagModel>> GetAsync(bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("Tag", () => _tagRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<TagModel>>(entity);
            return _mapper.Map<IEnumerable<TagModel>>(entity.Where(x => x.Active == active));
        }

        public async Task<TagModel> GetAsync(int id)
        {
            var entity = await _tagRepository.GetAsync(id);
            return _mapper.Map<TagModel>(entity);
        }
    }
}
