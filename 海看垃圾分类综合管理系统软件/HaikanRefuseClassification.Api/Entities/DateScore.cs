using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class DateScore
    {
        public int Id { get; set; }
        public Guid DateScoreUuid { get; set; }
        public int Jan { get; set; }
        public int Feb { get; set; }
        public int Mar { get; set; }
        public int Apr { get; set; }
        public int May { get; set; }
        public int Jun { get; set; }
        public int Jul { get; set; }
        public int Aug { get; set; }
        public int Sep { get; set; }
        public int Oct { get; set; }
        public int Nov { get; set; }
        public int Dec { get; set; }
        public string AddTime { get; set; }
        public Guid? HomeAddressUuid { get; set; }

        public virtual HomeAddress HomeAddressUu { get; set; }
    }
}
