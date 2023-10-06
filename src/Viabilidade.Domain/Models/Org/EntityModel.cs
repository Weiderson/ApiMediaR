﻿using Viabilidade.Domain.Entities.Org;
using Viabilidade.Domain.Models.Base;

namespace Viabilidade.Domain.Models.Org
{
    public class EntityModel : BaseModel
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public int OriginalEntityId { get; set; }
        public int SegmentId { get; set; }
        public virtual IEnumerable<ChannelModel> Channels { get; set; }
    }
}