
namespace Viabilidade.Domain.Entities.Alert
{
    public class AttachmentEntity : BaseEntity
    {
        public int? TreatmentId { get; set; }
        public virtual TreatmentEntity Treatment { get; set; }
        public string PathFile { get; set; }
        public DateTime? UploadDate { get; set; }
        public Guid? UserId { get; set; }
        public bool? Active { get; set; }
        public string FileName { get; set; }
    }
}
