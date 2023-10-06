namespace Viabilidade.Domain.Entities.Alert
{
    public class RTreatmentAlertEntity : BaseEntity
    {
        public int? AlertId { get; set; }
        public AlertEntity Alert { get; set; }
        public int? TreatmentId { get; set; }
        public TreatmentEntity Treatment { get; set; }
    }
}
