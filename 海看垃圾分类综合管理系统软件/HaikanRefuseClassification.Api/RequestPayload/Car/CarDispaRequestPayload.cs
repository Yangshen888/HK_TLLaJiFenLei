using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.RequestPayload.Car
{
    public class CarDispaRequestPayload:RequestPayload
    {
        public string kw1 { get; set; }
        public string kw2 { get; set; }
        public string[] kw3 { get; set; }
    }
}
