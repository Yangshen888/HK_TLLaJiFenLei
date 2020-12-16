using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class PerishRubbishViewTwo
    {
        public long? Id { get; set; }
        public string Ljname { get; set; }
        public string Towns { get; set; }
        public string Vname { get; set; }
        public Guid? GrabageRoomId { get; set; }
        public string Addtimes { get; set; }
        public double? DayData { get; set; }
        public double? Dataratio { get; set; }
        public double? Weekdata { get; set; }
        public double? Weekratio { get; set; }
        public double? Yeardata { get; set; }
        public double? Yearratio { get; set; }
    }
}
