using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class WingNoRecord
    {
        public long? Id { get; set; }
        public string Times { get; set; }
        public Guid GarbageRoomUuid { get; set; }
        public string Ljname { get; set; }
        public string Vname { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public string ColckDate { get; set; }
    }
}
