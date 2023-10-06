namespace Viabilidade.Domain.Entities.Alert
{
    public class AlgorithmEntity : BaseEntity
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public virtual IEnumerable<RuleEntity> Rules { get; set; }
    }
}