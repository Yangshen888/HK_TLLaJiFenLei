using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class RecycleInfo
    {
        public long? Id { get; set; }
        public Guid GarbageRoomUuid { get; set; }
        public string Ljname { get; set; }
        public string Vname { get; set; }
        public string Towns { get; set; }
        public string Lon { get; set; }
        public string Lat { get; set; }
        public Guid? GrabageRoomId { get; set; }
        public string Type { get; set; }
    }
}
