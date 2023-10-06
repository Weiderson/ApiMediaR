using AutoMapper;
using Viabilidade.Domain.Entities.Alert;
using Viabilidade.Domain.Models.Alert;

namespace Viabilidade.Service.Mappers.Alertas
{
    public class EntityToModel : Profile
    {
        public EntityToModel()
        {
            CreateMap<FavoriteAlertEntity, FavoriteAlertModel>().ReverseMap();
            CreateMap<AlertEntity, AlertModel>().ReverseMap();
            CreateMap<AlertChannelEntity, AlertChannelModel>().ReverseMap();
            CreateMap<AlgorithmEntity, AlgorithmModel>().ReverseMap();
            CreateMap<TreatmentClassEntity, TreatmentClassModel>().ReverseMap();
            CreateMap<OperatorEntity, OperatorModel>().ReverseMap();
            CreateMap<ParameterEntity, ParameterModel>().ReverseMap();
            CreateMap<RuleEntity, RuleModel>().ReverseMap();
            CreateMap<StatusEntity, StatusModel>().ReverseMap();
            CreateMap<TagEntity, TagModel>().ReverseMap();
            CreateMap<TreatmentTypeEntity, TreatmentTypeModel>().ReverseMap();
            CreateMap<IndicatorEntity, IndicatorModel>().ReverseMap();
            CreateMap<IndicatorFilterEntity, IndicatorFilterModel>().ReverseMap();
            CreateMap<EntityRuleEntity, EntityRuleModel>().ReverseMap();
            CreateMap<TreatmentEntity, TreatmentModel>().ReverseMap();
            CreateMap<AttachmentEntity, AttachmentModel>().ReverseMap();
            CreateMap<RChannelEntityRuleEntity, RChannelEntityRuleModel>().ReverseMap();
            CreateMap<TagAlertEntity, TagAlertModel>().ReverseMap();
            CreateMap<SilencedAlertEntity, SilencedAlertModel>().ReverseMap();
            CreateMap<RTreatmentAlertEntity, RTreatmentAlertModel>().ReverseMap();
        }
    }
}