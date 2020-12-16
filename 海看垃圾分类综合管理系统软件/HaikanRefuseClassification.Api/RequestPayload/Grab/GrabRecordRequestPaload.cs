using HaikanRefuseClassification.Api.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.RequestPayload.Grab
{
    public class GrabRecordRequestPaload : RequestPayload
    {
        public CommonEnum.IsDeleted IsDeleted { get; set; }
        public string Kw { get; set; }
        public string Kw1 { get; set; }
        public string[] Kw2 { get; set; }
        public List<string> time { get; set; }
        public string vuuid { get; set; }
        public string street { get; set; }
        public string ccmmunity { get; set; }
    }
}
