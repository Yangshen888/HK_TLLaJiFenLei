using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class Leaderboard
    {
        public long? Id { get; set; }
        public Guid SystemUserUuid { get; set; }
        public Guid? HomeAddressUuid { get; set; }
        public string LoginName { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string Ccmmunity { get; set; }
        public int? Zyzsc { get; set; }
    }
}
