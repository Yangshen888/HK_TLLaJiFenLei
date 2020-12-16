using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class DayOfWeight
    {
        public long? Id { get; set; }
        public Guid? GrabageRoomId { get; set; }
        public string Wtime { get; set; }
        public decimal Qtlj { get; set; }
        public decimal Yflj { get; set; }
        public decimal? Proportion { get; set; }
    }
}
