using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.RequestPayload.Grab
{
    public class WeighRequestPayload : RequestPayload
    {
        public string kw1 { get; set; }
        public string kw3 { get; set; }
        public string[] kw2 { get; set; }
        public string vuuid { get; set; }
    }
}
