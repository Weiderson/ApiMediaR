
namespace Viabilidade.Domain.Entities.Alert
{
    public class ParameterEntity : BaseEntity
    {
        public bool? Active { get; set; }
        public decimal? LowSeverity { get; set; }
        public decimal? MediumSeverity { get; set; }
        public decimal? HighSeverity { get; set; }
        //public decimal? IndicatorPeriod { get; set; }
        public decimal? EvaluationPeriod { get; set; }
        public decimal? ComparativePeriod { get; set; }
        public virtual IEnumerable<RuleEntity> Rules { get; set; }
        public virtual IEnumerable<EntityRuleEntity> EntityRules { get; set; }

    }
}