using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class RubbishSyrecord
    {
        public Guid GarbageSyuuid { get; set; }
        public int Id { get; set; }
        public Guid? GrabageRoomUuid { get; set; }
        public Guid? VillageUuid { get; set; }
        public string Sytime { get; set; }
        public string RubbishType { get; set; }
        public string IsDelete { get; set; }
        public string State { get; set; }
        public string CorruptRubbishPercent { get; set; }
        public Guid? GrabageWeighingUuid { get; set; }
        public Guid? CarUuid { get; set; }

        public virtual GrabageRoom GrabageRoomUu { get; set; }
        public virtual GrabageWeighting GrabageWeighingUu { get; set; }
        public virtual Village VillageUu { get; set; }
    }
}
