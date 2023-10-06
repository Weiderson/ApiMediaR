namespace Viabilidade.Domain.Entities.Alert
{
    public class TagAlertEntity : BaseEntity
    {
        public int RuleId { get; set; }
        public virtual RuleEntity Rule { get; set; }
        public int TagId { get; set; }
        public virtual TagEntity Tag { get; set; }
        public bool Active { get; set; }
    }
}