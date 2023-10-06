﻿using AutoMapper;
using System;
using System.Reflection;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Interfaces.Notifications;
using Viabilidade.Domain.Interfaces.Repositories.Alert;
using Viabilidade.Domain.Interfaces.Services.Alert;
using Viabilidade.Domain.Models.Alert;
using Viabilidade.Domain.Notifications;

namespace Viabilidade.Service.Services.Alert
{
    public class FavoriteAlertService : IFavoriteAlertService
    {
        private readonly IFavoriteAlertRepository _alertaFavoritoRepository;
        private readonly IRuleRepository _regraRepository;
        private readonly INotificationHandler<Notification> _notification;
        private readonly IMapper _mapper;
        public FavoriteAlertService(IFavoriteAlertRepository alertaFavoritoRepository, IRuleRepository regraRepository, INotificationHandler<Notification> notification, IMapper mapper)
        {
            _alertaFavoritoRepository = alertaFavoritoRepository;
            _regraRepository = regraRepository;
            _notification = notification;
            _mapper = mapper;
        }

        public async Task<FavoriteAlertModel> FavoriteAsync(FavoriteAlertModel model)
        {
            await ValidateFavorite((int)model.RuleId);
            if(_notification.HasNotification())
                return null;

            model.UpdateDate = DateTime.Now;
            model.Active = true;
            var entity = await _alertaFavoritoRepository.CreateAsync(_mapper.Map<FavoriteAlertEntity>(model));
            return _mapper.Map<FavoriteAlertModel>(entity);
        }

        private async Task ValidateFavorite(int ruleId)
        {
            if (await _regraRepository.GetAsync(ruleId) == null)
                _notification.AddNotification(400, "Regra não encontrada");
            if (await _alertaFavoritoRepository.ExistFavoriteAsync(ruleId))
                _notification.AddNotification(400, "Regra já está favoritada");
        }

        public async Task<FavoriteAlertModel> GetAsync(int id)
        {
            var entity = await _alertaFavoritoRepository.GetAsync(id);
            return _mapper.Map<FavoriteAlertModel>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _alertaFavoritoRepository.DeleteAsync(id);
        }

        public async Task<FavoriteAlertModel> UnFavoriteAsync(int ruleId)
        {
            var favorito = await _alertaFavoritoRepository.GetByRuleUserAsync(ruleId);
            if (favorito == null)
            {
                _notification.AddNotification(403, "Alteração não permitida");
                return null;
            }

            favorito.UpdateDate = DateTime.Now;
            favorito.Active = false;
            var entity = await _alertaFavoritoRepository.UpdateAsync(favorito.Id, favorito);
            return _mapper.Map<FavoriteAlertModel>(entity);

        }
    }
}
