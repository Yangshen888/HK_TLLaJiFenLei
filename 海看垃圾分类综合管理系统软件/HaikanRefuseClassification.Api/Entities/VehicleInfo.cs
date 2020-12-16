using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class VehicleInfo
    {
        public Guid VehicleInfo1 { get; set; }
        public Guid? CarUuid { get; set; }
        public string Addtime { get; set; }
        public string Lon { get; set; }
        public string Lat { get; set; }
        public int Id { get; set; }
    }
}
