using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class StreeStatistical
    {
        public long? Id { get; set; }
        public string Towns { get; set; }
        public string Vname { get; set; }
        public string Times { get; set; }
        public double? Daysum { get; set; }
        public double? Dayprop { get; set; }
        public double? Weekprop { get; set; }
        public double? Weekyf { get; set; }
        public double? Weeksum { get; set; }
        public double? Yearsum { get; set; }
        public double? Yearprop { get; set; }
        public DateTime? Weekstr { get; set; }
        public DateTime? Weekend { get; set; }
    }
}
