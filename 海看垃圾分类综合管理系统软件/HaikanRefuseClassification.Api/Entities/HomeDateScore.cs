using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class HomeDateScore
    {
        public long? Id { get; set; }
        public Guid HomeAddressUuid { get; set; }
        public double? Score { get; set; }
        public int LastDecScore { get; set; }
        public int JanScore { get; set; }
        public int FebScore { get; set; }
        public int MarScore { get; set; }
        public int AprScore { get; set; }
        public int MayScore { get; set; }
        public int JunScore { get; set; }
        public int JulScore { get; set; }
        public int AugScore { get; set; }
        public int SepScore { get; set; }
        public int OctScore { get; set; }
        public int NovScore { get; set; }
        public int DecScore { get; set; }
        public int LastDec { get; set; }
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
    }
}
