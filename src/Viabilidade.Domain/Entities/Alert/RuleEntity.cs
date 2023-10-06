using System.Reflection.Metadata;

namespace Viabilidade.Domain.Entities.Alert
{
    public class RuleEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AlgorithmId { get; set; }
        public virtual AlgorithmEntity Algorithm { get; set; }
        public int? IndicatorId { get; set; }
        public virtual IndicatorEntity Indicator { get; set; }
        public int? OperatorId { get; set; }
        public virtual OperatorEntity Operator { get; set; }
        public int? IndicatorFilterId { get; set; }
        public virtual IndicatorFilterEntity IndicatorFilter { get; set; }
        public int? ParameterId { get; set; }
        public virtual ParameterEntity Parameter { get; set; }
        public bool? Active { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public Guid? UserId { get; set; }
        public int? VersionMajor { get; set; }
        public int? VersionMinor { get; set; }
        public int? VersionPatch { get; set; }
        public virtual IEnumerable<FavoriteAlertEntity> FavoriteAlerts { get; set; }
        public virtual IEnumerable<TagAlertEntity> TagAlerts { get; set; }
        public virtual IEnumerable<EntityRuleEntity> EntityRules { get; set; }

    }
}
