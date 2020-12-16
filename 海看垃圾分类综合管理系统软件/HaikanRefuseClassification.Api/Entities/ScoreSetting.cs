using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class ScoreSetting
    {
        public ScoreSetting()
        {
            GrabageDisposal = new HashSet<GrabageDisposal>();
            GrabageDisposalCopy1 = new HashSet<GrabageDisposalCopy1>();
        }

        public Guid ScoreUuid { get; set; }
        public string ScoreName { get; set; }
        public int? Integral { get; set; }
        public string Addpeople { get; set; }
        public string AddTime { get; set; }

        public virtual ICollection<GrabageDisposal> GrabageDisposal { get; set; }
        public virtual ICollection<GrabageDisposalCopy1> GrabageDisposalCopy1 { get; set; }
    }
}
