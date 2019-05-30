using System;
using System.Collections.Generic;
using System.Text;

namespace Friends.Domain
{
    public class FriendShipEdge
    {
        public string id { get; set; }
        public string SourceId { get; set; }
        public string TargetId { get; set; }
    }
}
