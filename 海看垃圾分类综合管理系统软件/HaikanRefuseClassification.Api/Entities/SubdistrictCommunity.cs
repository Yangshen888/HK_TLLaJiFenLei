using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class SubdistrictCommunity
    {
        public long? Id { get; set; }
        public string Towns { get; set; }
        public string Vname { get; set; }
        public string Resregion { get; set; }
    }
}
