using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.RequestPayload.Base
{
    public class VehicleRequestPayload : RequestPayload
    {
        public string kw { get; set; }
    }
}
