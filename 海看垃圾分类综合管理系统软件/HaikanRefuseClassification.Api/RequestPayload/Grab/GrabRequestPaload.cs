using HaikanRefuseClassification.Api.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.RequestPayload.Grab
{
    public class GrabRequestPaload:RequestPayload
    {
        public CommonEnum.IsDeleted IsDeleted { get; set; }
        public List<string> time { get; set; }
        public string Kw1 { get; set; }
        public string MarkType { get; set; }
        public string vuuid { get; set; }
        public string isScore { get; set; }
        public string street { get; set; }
        public string ccmmunity { get; set; }
        public Guid? Gruuid { get; set; }
    }
}
