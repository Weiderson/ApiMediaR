namespace Viabilidade.Domain.Entities.Alert
{
    public class TagEntity : BaseEntity
    {
        public string Name { get; set; }
        public int OriginalId { get; set; }
        public bool Active { get; set; }
        public virtual IEnumerable<TagAlertEntity> TagAlerts { get; set; }
    }
}