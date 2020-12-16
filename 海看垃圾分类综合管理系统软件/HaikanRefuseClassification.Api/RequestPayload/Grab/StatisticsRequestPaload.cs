using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.RequestPayload.Grab
{
    public class StatisticsRequestPaload : RequestPayload
    {
        public int Time { get; set; }
        public string vuuid { get; set; }
    }
}
