namespace Viabilidade.Domain.Entities.Alert
{
    public class TreatmentTypeEntity : BaseEntity
    {
        public string Description { get; set; }
        public int TreatmentClassId { get; set; }
        public virtual TreatmentClassEntity TreatmentClass { get; set; }
        public virtual IEnumerable<TreatmentEntity> Treatments { get; set; }
        public string TreatmentTypeConcept { get; set; }
        public bool Active { get; set; }
    }
}
