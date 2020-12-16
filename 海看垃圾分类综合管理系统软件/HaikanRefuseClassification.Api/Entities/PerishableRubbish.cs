using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class PerishableRubbish
    {
        public long? Id { get; set; }
        public Guid VillageUuid { get; set; }
        public double? Num { get; set; }
        public int? Ye { get; set; }
        public int? Mon { get; set; }
    }
}
