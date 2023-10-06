
namespace Viabilidade.Domain.Entities.Alert
{
    public class StatusEntity : BaseEntity
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public virtual IEnumerable<AlertEntity> Alerts { get; set; }
    }
}
