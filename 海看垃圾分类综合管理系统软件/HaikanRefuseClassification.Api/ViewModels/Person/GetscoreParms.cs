using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels.Person
{
    public class GetscoreParms
    {

        public string uuid { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string trashtype { get; set; }
        public string pa { get; set; }
    }
}
