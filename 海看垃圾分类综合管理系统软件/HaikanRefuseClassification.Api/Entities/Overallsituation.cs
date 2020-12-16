using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class Overallsituation
    {
        public Guid SetUuid { get; set; }
        public string SetName { get; set; }
        public int? SetNumber { get; set; }
        public int HourScore { get; set; }
    }
}
