namespace Viabilidade.Domain.Entities.Alert
{
    public class TreatmentClassEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Concept { get; set; }
        public bool Active { get; set; }

        public virtual IEnumerable<TreatmentEntity> Treatments { get; set; }
        public virtual IEnumerable<TreatmentTypeEntity> TreatmentTypes { get; set; }
    }
}