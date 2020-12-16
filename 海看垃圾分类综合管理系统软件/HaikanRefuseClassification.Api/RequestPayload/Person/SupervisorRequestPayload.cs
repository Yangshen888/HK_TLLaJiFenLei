using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.RequestPayload.Person
{
    public class SupervisorRequestPayload : RequestPayload
    {
        public string kw { get; set; }
        public string kw1 { get; set; }
        public string kw2 { get; set; }
        public string vuuid { get; set; }
    }
}
