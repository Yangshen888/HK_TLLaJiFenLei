using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class GrabageWeightSon
    {
        public Guid GrabageWeighingRecordUuid { get; set; }
        public int Id { get; set; }
        public Guid? GrabageRoomId { get; set; }
        public string CarNumber { get; set; }
        public string AddTime { get; set; }
        public string Weight { get; set; }
        public string Type { get; set; }
        public string RecordType { get; set; }
        public string IsDelete { get; set; }
        public Guid? CarUuid { get; set; }
        public string CheckedUser { get; set; }
        public int? Grade { get; set; }
        public int? IsCheck { get; set; }
        public string WeightTime { get; set; }
        public Guid? WeightUuid { get; set; }

        public virtual Car CarUu { get; set; }
        public virtual GrabageRoom GrabageRoom { get; set; }
    }
}
