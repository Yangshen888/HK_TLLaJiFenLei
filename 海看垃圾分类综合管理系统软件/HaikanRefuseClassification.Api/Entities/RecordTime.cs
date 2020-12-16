using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class RecordTime
    {
        public string Times { get; set; }
        public Guid GarbageRoomUuid { get; set; }
    }
}
