namespace Viabilidade.Domain.Entities.Alert
{
    public class OperatorEntity : BaseEntity
    {
        public string Description { get; set; }
        public string Command { get; set; }
        public bool Active { get; set; }
        public virtual IEnumerable<RuleEntity> Rules { get; set; }
    }
}