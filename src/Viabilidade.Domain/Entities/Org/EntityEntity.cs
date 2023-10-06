
namespace Viabilidade.Domain.Entities.Org
{
    public class EntityEntity : BaseEntity
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public int OriginalEntityId { get; set; }
        public int SegmentId { get; set; }
        public virtual IEnumerable<ChannelEntity> Channels { get; set; }
    }
}