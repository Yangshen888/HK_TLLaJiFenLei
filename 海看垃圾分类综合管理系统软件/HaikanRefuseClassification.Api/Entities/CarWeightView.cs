using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class CarWeightView
    {
        public long? Id { get; set; }
        public string CarNumber { get; set; }
        public string GrabType { get; set; }
        public string Street { get; set; }
        public Guid CarUuid { get; set; }
        public string Wtime { get; set; }
        public decimal? Weight { get; set; }
    }
}
