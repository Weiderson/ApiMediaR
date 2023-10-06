
using Viabilidade.Domain.Entities.Org;

namespace Viabilidade.Domain.Entities.Alert
{
    public class AlertChannelEntity : BaseEntity
    {
        public int AlertId { get; set; }
        public virtual AlertEntity Alert { get; set; }
        public int ChannelId { get; set; }
        public virtual ChannelEntity Channel { get; set; }
        public string ChannelName { get; set; }
    }
}
