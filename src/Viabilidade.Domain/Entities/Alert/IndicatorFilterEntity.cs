namespace Viabilidade.Domain.Entities.Alert
{
    public class IndicatorFilterEntity : BaseEntity
    {
        public string Description { get; set; }
        public string Command { get; set; }
        public bool Active { get; set; }
        public virtual IEnumerable<RuleEntity> Rules { get; set; }
        public virtual IEnumerable<EntityRuleEntity> EntityRules { get; set; }
    }
}
