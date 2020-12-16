using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.RequestPayload.Base
{
    public class WingWarnRequestPayload:RequestPayload
    {
        public string Kw1 { get; set; }
        public List<string> Kw2 { get; set; }
    }
}
