using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Material
{
    public class ConsumableEditView
    {
        public Guid ConsumableUuid { get; set; }
        public int? MaterialType { get; set; }
        public string MaterialName { get; set; }
        public int? Num { get; set; }
        public string Specification { get; set; }
        public string Remark { get; set; }
        public string RealName { get; set; }
    }
}
