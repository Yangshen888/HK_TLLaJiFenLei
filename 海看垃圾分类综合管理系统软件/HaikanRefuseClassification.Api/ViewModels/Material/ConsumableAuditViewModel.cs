using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Material
{
    public class ConsumableAuditViewModel
    {
        public Guid ConsumableUuid { get; set; }
        
        public string AuditOpinion { get; set; }
    }
}
