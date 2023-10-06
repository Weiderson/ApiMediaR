namespace Viabilidade.Domain.Entities.Alert
{
    public class SilencedAlertEntity : BaseEntity
    {
        public int AlertId { get; set; }
        public virtual AlertEntity Alert { get; set; }
        public DateTime StartDateSilence { get; set; }
        public DateTime EndDateSilence { get; set; }
        public bool Active { get; set; }
    }
}
