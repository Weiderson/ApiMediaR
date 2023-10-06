using Viabilidade.Domain.Entities.Org;

namespace Viabilidade.Domain.Entities.Alert
{
    public class AlertEntity: BaseEntity
    {
        public int? EntityId { get; set; }
        public string EntityName { get; set; }
        public EntityEntity Entity { get; set; }
        public int EntityRuleId { get; set; }
        public virtual EntityRuleEntity EntityRule { get; set; }
        public string RuleName { get; set; }
        public DateTime? Version { get; set; }
        public DateTime? IndicatorFirstDate { get; set; }
        public DateTime? IndicadorLastDate { get; set; }
        public string Severity { get; set; }
        public decimal? Indicator { get; set; }
        public decimal? LowReferenceIndicator { get; set; }
        public decimal? MediumReferenceIndicator { get; set; }
        public decimal? HighReferenceIndicator { get; set; }
        public int? StatusId { get; set; }
        public virtual StatusEntity Status { get; set; }
        public virtual IEnumerable<AlertChannelEntity> AlertChannels { get; set; }
        public virtual IEnumerable<SilencedAlertEntity> SilencedAlerts { get; set; }
        public virtual IEnumerable<TreatmentEntity> Treatments { get; set; }
        public DateTime? FinishDate { get; set; }
        public Guid? UserId { get; set; }
        public bool? Active { get; set; }
        public bool? Treated { get; set; }
        public decimal? IndicatorValue { get; set; }
        public decimal? PercentageIndicator { get; set; }
    }
}
