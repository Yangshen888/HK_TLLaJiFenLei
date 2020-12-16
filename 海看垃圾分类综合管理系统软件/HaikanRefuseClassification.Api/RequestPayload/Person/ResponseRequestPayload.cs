using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.RequestPayload.Person
{
    public class ResponseRequestPayload : RequestPayload
    {
        public string kw { get; set; }
        public string vuuid { get; set; }
        public string street { get; set; }
        public string ccmmunity { get; set; }
    }
}
