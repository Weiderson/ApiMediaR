namespace Viabilidade.Domain.Entities.Org
{
    public class ChannelEntity : BaseEntity
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public int OriginalChannelId{ get; set; }
        public int SubgroupId { get; set; }
    }
}
