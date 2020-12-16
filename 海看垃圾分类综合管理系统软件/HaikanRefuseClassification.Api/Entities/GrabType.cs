using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class GrabType
    {
        public Guid GrabTypeUuid { get; set; }
        public int Id { get; set; }
        public string GrabName { get; set; }
        public string IsDelete { get; set; }
        public string AddTime { get; set; }
        public string AddPeopel { get; set; }
        public string Type { get; set; }
    }
}
