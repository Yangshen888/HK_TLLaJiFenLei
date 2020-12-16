using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.RequestPayload.Monitoring
{
    public class MonitoringRequestPayload : RequestPayload
    {
        public string kw1 { get; set; }
        public string vuuid { get; set; }
        public string street { get; set; }
        public string ccmmunity { get; set; }
    }
}
