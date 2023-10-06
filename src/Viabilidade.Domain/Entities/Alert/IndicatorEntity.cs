using Viabilidade.Domain.Entities.Org;

namespace Viabilidade.Domain.Entities.Alert
{
    public class IndicatorEntity : BaseEntity
    {
        public string Description { get; set; }
        public string Command { get; set; }
        public bool Active { get; set; }
        public string SQLIndicator { get; set; }
        public int SegmentId { get; set; }
        public virtual SegmentEntity Segment { get; set; }
        public virtual IEnumerable<RuleEntity> Rules { get; set; }
    }
}
