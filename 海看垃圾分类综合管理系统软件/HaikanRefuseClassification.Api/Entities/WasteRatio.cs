using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class WasteRatio
    {
        public long? Id { get; set; }
        public string Vname { get; set; }
        public decimal? Eflj { get; set; }
        public decimal? Qtlj { get; set; }
        public decimal? Waste { get; set; }
        public decimal? Per { get; set; }
    }
}
