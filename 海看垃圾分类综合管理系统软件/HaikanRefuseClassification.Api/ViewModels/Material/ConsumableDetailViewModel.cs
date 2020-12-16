using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Material
{
    public class ConsumableDetailViewModel
    {
        public Guid ConsumableUuid { get; set; }
        public string ApplyTime { get; set; }
        public int? MaterialType { get; set; }
        public string MaterialName { get; set; }
        public int? Num { get; set; }
        public string Specification { get; set; }
        public string Remark { get; set; }
        public string RealName { get; set; }
        public string AuditOpinion { get; set; }

    }
}
