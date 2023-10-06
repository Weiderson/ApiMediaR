
namespace Viabilidade.Domain.Entities.Alert
{
    public class TreatmentEntity : BaseEntity
    {
        public DateTime? Date { get; set; }
        public Guid? UserId { get; set; }
        public int? TreatmentClassId { get; set; }
        public virtual TreatmentClassEntity TreatmentClass { get; set; }
        public int? TreatmentTypeId { get; set; }
        public virtual TreatmentTypeEntity TreatmentType { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public virtual IEnumerable<AttachmentEntity> Attachments { get; set; }
        public virtual IEnumerable<AlertEntity> Alerts { get; set; }
    }
}
