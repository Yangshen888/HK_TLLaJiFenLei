using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.RequestPayload.Person
{
    public class AddressRequestPayload : RequestPayload
    {
        public string Kw { get; set; }//地址名
        public string Kw1 { get; set; }
        public string Kw2 { get; set; }
        public string Kw3 { get; set; }
        public string vuuid { get; set; }
        public string vill { get; set; }
    }
}
