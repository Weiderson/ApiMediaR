using AutoMapper;
using System;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Interfaces.Cache;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Models.Alert;

namespace Viabilidade.Service.Services.Alert
{
    public class TagAlertService : ITagAlertService
    {

        private readonly ITagAlertRepository _alertaTagRepository;
        private readonly IStorageCache _cache;
        private readonly IMapper _mapper;
        public TagAlertService(ITagAlertRepository alertaTagRepository, IStorageCache cache, IMapper mapper)
        {
            _alertaTagRepository = alertaTagRepository;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<TagAlertModel> CreateAsync(TagAlertModel model)
        {
            var entity = await _alertaTagRepository.CreateAsync(_mapper.Map<TagAlertEntity>(model));
            return _mapper.Map<TagAlertModel>(entity);
        }

        public async Task<bool> DeleteByRuleAsync(int ruleId)
        {
            return await _alertaTagRepository.DeleteByRuleAsync(ruleId);
        }

        public async Task<IEnumerable<TagAlertModel>> GetAsync(bool? active)
        {
            var entity = await _cache.GetOrCreateAsync("TagAlert", () => _alertaTagRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<TagAlertModel>>(entity);
            return _mapper.Map<IEnumerable<TagAlertModel>>(entity.Where(x => x.Active == active));
        }

        public async Task<TagAlertModel> GetAsync(int id)
        {
            var entity = await _alertaTagRepository.GetAsync(id);
            return _mapper.Map<TagAlertModel>(entity);
        }

        public async Task<IEnumerable<TagAlertModel>> GetByRuleAsync(int ruleId, bool? active = null)
        {
            var entity = await _cache.GetOrCreateAsync("TagAlert", () => _alertaTagRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<TagAlertModel>>(entity.Where(x => x.RuleId == ruleId));
            return _mapper.Map<IEnumerable<TagAlertModel>>(entity.Where(x => x.RuleId == ruleId && x.Active == active));
        }

        public async Task<IEnumerable<TagAlertModel>> GetByTagAsync(int tagId, bool? active = null)
        {
            var entity = await _cache.GetOrCreateAsync("TagAlert", () => _alertaTagRepository.GetAsync());
            if (active == null)
                return _mapper.Map<IEnumerable<TagAlertModel>>(entity.Where(x => x.TagId == tagId));
            return _mapper.Map<IEnumerable<TagAlertModel>>(entity.Where(x => x.TagId == tagId && x.Active == active));
        }



    }
}
