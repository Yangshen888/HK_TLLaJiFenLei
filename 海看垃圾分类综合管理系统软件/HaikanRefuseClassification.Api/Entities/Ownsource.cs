using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class Ownsource
    {
        public long? Id { get; set; }
        public Guid HomeAddressUuid { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string Ccmmunity { get; set; }
        public string Resregion { get; set; }
        public int Used { get; set; }
        public int Alls { get; set; }
        public int? Can { get; set; }
        public int LastScore { get; set; }
        public int NewScore { get; set; }
        public int State { get; set; }
    }
}
