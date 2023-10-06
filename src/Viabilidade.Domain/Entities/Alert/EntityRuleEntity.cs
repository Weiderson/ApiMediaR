using Viabilidade.Domain.Entities.Org;

namespace Viabilidade.Domain.Entities.Alert
{
    public class EntityRuleEntity: BaseEntity
    {
        public int? RuleId { get; set; }
        public virtual RuleEntity Rule { get; set; }
        public int? EntityId { get; set; }
        public EntityEntity Entity { get; set; }
        public string SubEntityNumber { get; set; }
        public Guid? UserId { get; set; }
        public int? IndicatorFilterId { get; set; }
        public virtual IndicatorFilterEntity IndicatorFilter { get; set; }
        public int? ParameterId { get; set; }
        public virtual ParameterEntity Parameter { get; set; }
        public bool? Active { get; set; }
        public virtual IEnumerable<AlertEntity> Alerts { get; set; }
        public virtual IEnumerable<ChannelEntity> Channels { get; set; }
    }
}
