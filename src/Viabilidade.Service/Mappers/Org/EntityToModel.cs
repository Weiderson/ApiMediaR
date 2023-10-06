using AutoMapper;
using Viabilidade.Domain.Entities.Org;
using Viabilidade.Domain.Entities.Views;
using Viabilidade.Domain.Models.Org;
using Viabilidade.Domain.Models.Views;

namespace Viabilidade.Service.Mappers.Org
{
    public class EntityToModel : Profile
    {
        public EntityToModel()
        {
            CreateMap<ChannelEntity, ChannelModel>().ReverseMap();
            CreateMap<EntityEntity, EntityModel>().ReverseMap();
            CreateMap<SegmentEntity, SegmentModel>().ReverseMap();
            CreateMap<SquadEntity, SquadModel>().ReverseMap();
            CreateMap<SubgroupEntity, SubgroupModel>().ReverseMap();
            CreateMap<BondEntity, BondModel>().ReverseMap();
        }
    }
}